using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Threading;
using AOP.WindowsService.FileHandler.Infrastructure;
using AOP.WindowsService.FileHandler.Interfaces;
using NLog;

namespace AOP.WindowsService.FileHandler.Services
{
    public class FileHandler : IFileHandler
    {
        private volatile int _sequenceNumber = 0;
        private ConcurrentBag<string> _sequenceFileNames;

        private readonly INameHelper _nameHelper;
        private readonly IPdfService _pdfService;
        private readonly ILogger _logger;

        private int _timeout;

        private FileSystemWatcher _fileSystemWatcher;
        private Timer _savingTimer;

        public FileHandler(INameHelper nameHelper, IPdfService pdfService, ILogger logger)
        {
            _nameHelper = nameHelper;
            _pdfService = pdfService;
            _logger = logger;
            _sequenceFileNames = new ConcurrentBag<string>();
            _savingTimer = new Timer(TimerCallback);
            _fileSystemWatcher = new FileSystemWatcher();
            _fileSystemWatcher.IncludeSubdirectories = false;
        }

        [LoggingAspect]
        public void Start(string sourceFolderPath, int saveTimeout)
        {
            _logger.Info("The file watcher is starting.");

            _timeout = saveTimeout;
            _savingTimer.Change(_timeout, _timeout);

            _fileSystemWatcher.Path = sourceFolderPath;
            _fileSystemWatcher.Created += FileSystemWatcher_Created;
            _fileSystemWatcher.EnableRaisingEvents = true;

            try
            {
                Initialize(sourceFolderPath);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                throw;
            }

            _logger.Info("The file watcher has started.");
        }

        [LoggingAspect]
        public void Stop()
        {
            _logger.Info("The file watcher is stopping.");

            _savingTimer.Change(Timeout.Infinite, 0);

            _fileSystemWatcher.EnableRaisingEvents = false;
            _fileSystemWatcher.Created -= FileSystemWatcher_Created;

            SaveDocument();

            _logger.Info("The file watcher has stopped.");
        }

        [LoggingAspect]
        private void FileSystemWatcher_Created(object sender, FileSystemEventArgs e)
        {
            if (!File.Exists(e.FullPath) || !_nameHelper.IsNameMatch(e.Name))
            {
                return;
            }

            var number = _nameHelper.GetFileNameNumber(e.Name);

            if (number - _sequenceNumber != 1)
            {
                _logger.Debug(@"""Jumping"" numbering has been detected. Saving the sequence to a document.");
                SaveDocument();
            }

            _logger.Debug("Adding new file to the sequence.");

            _sequenceFileNames.Add(e.FullPath);
            _sequenceNumber = number;

            _savingTimer.Change(_timeout, _timeout);
        }

        [LoggingAspect]
        private void SaveDocument()
        {
            try
            {
                var sequence = Interlocked.Exchange(ref _sequenceFileNames, new ConcurrentBag<string>());
                _pdfService.SaveDocument(sequence.Reverse(), GenerateUniqueFileName());
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                throw;
            }
        }

        [LoggingAspect]
        private string GenerateUniqueFileName()
        {
            return Guid.NewGuid().ToString();
        }

        [LoggingAspect]
        private void TimerCallback(object target)
        {
            _logger.Debug("The next page has timed out.");
            SaveDocument();
        }

        [LoggingAspect]
        private void Initialize(string sourceFolderPath)
        {
            foreach (var file in Directory.GetFiles(sourceFolderPath))
            {
                var eventArgs = new FileSystemEventArgs(
                    WatcherChangeTypes.Created,
                    Path.GetDirectoryName(file),
                    Path.GetFileName(file));

                FileSystemWatcher_Created(this, eventArgs);
            }
        }
    }
}
