using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;
using MSMQ.StreamScanning.CentralService.Interfaces;
using MSMQ.StreamScanning.Common.Interfaces;
using MSMQ.StreamScanning.Common.Models;
using NLog;

namespace MSMQ.StreamScanning.CentralService.Services
{
    public class CommandSender : ICommandSender
    {
        private readonly ILogger _logger;
        private readonly ICommandParser _commandParser;
        private readonly IMessageQueueFactory _msmqFactory;
        private ConcurrentBag<IMessageQueueSender> _msmqSenders;
        private readonly FileSystemWatcher _fileSystemWatcher;
        private readonly string _сommandFile;
        private readonly int _retryCount = 5;
        private readonly int _retryDelay = 1000;

        public CommandSender(IMessageQueueFactory msmqFactory, ICommandParser commandParser, ISettingsProvider settingsProvider, ILogger logger)
        {
            _msmqFactory = msmqFactory;
            _msmqSenders = new ConcurrentBag<IMessageQueueSender>();

            _commandParser = commandParser;

            _сommandFile = settingsProvider.GetCommandFileName();
            _fileSystemWatcher = new FileSystemWatcher();
            _fileSystemWatcher.IncludeSubdirectories = false;

            _logger = logger;
        }

        public void Start()
        {
            _fileSystemWatcher.Path = Path.GetDirectoryName(Path.GetFullPath(_сommandFile));
            _fileSystemWatcher.Filter = Path.GetFileName(_сommandFile);
            _fileSystemWatcher.Changed += OnCommandFileChanged;
            _fileSystemWatcher.EnableRaisingEvents = true;
        }

        public void Stop()
        {
            _fileSystemWatcher.EnableRaisingEvents = false;
            _fileSystemWatcher.Changed -= OnCommandFileChanged;

            Interlocked.Exchange(ref _msmqSenders, new ConcurrentBag<IMessageQueueSender>());
        }

        public void AddSubscriber(string queuePath)
        {
            _msmqSenders.Add(_msmqFactory.GetSender(queuePath));
        }

        private void OnCommandFileChanged(object sender, FileSystemEventArgs e)
        {
            _fileSystemWatcher.EnableRaisingEvents = false;

            OpenFileWithRetries(e.FullPath, reader =>
            {
                string line = reader.ReadLine();
                var command = _commandParser.Parse(line);

                if (command == null)
                {
                    _logger.Warn($"The command has not been recognized: '{line}'");
                }

                Publish(command);
            });

            _fileSystemWatcher.EnableRaisingEvents = true;
        }
        
        private void Publish(ICentralCommand command)
        {
            foreach (var sender in _msmqSenders)
            {
                sender.Send(command);
            }
        }

        private void OpenFileWithRetries(string path, Action<StreamReader> action)
        {
            StreamReader streamReader = null;

            _logger.Debug($"Starting opening the file {path}.");

            for (int i = 0; i < _retryCount; i++)
            {
                try
                {
                    _logger.Debug($"{path} - attempt {i}.");
                    streamReader = File.OpenText(path);
                    break;
                }
                catch (IOException ex)
                {
                    _logger.Warn($"Unable to open the file {path}. Waiting for {_retryDelay} milliseconds. Exception message: {ex.Message}");
                    Thread.Sleep(_retryDelay);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex.Message);
                    throw;
                }
            }

            if (streamReader == null)
            {
                var message = $"Unable to open the file {path}.";
                _logger.Error(message);
                throw new IOException(message);
            }

            _logger.Debug($"The file {path} has been opened.");

            action(streamReader);

            streamReader.Close();
            streamReader.Dispose();
        }
    }
}
