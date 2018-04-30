using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Threading;
using MSMQ.StreamScanning.Interfaces;
using MSMQ.StreamScanning.Models;
using NLog;

namespace MSMQ.StreamScanning.Services
{
    public class FileHandler : IFileHandler
    {
        private volatile ServiceActivity _currentActivity;
        private volatile int _sequenceNumber = 0;
        private ConcurrentBag<string> _sequenceFileNames;

        private readonly INameHelper _nameHelper;
        private readonly IPdfService _pdfService;
        private readonly ILogger _logger;

        private int _timeout;

        private FileSystemWatcher _fileSystemWatcher;
        private Timer _savingTimer;

        public event EventHandler<DocumentEventArgs> DocumentSaved;

        public ServiceActivity CurrentActivity => _currentActivity;

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

        public void Start(string sourceFolderPath, int saveTimeout)
        {
            _logger.Info("The file watcher is starting.");

            ChangeStatus(ServiceActivity.Starting);

            _timeout = saveTimeout * 1000;
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

            ChangeStatus(ServiceActivity.Waiting);

            _logger.Info("The file watcher has started.");
        }

        public void Stop()
        {
            _logger.Info("The file watcher is stopping.");

            ChangeStatus(ServiceActivity.Stopping);

            _savingTimer.Change(Timeout.Infinite, 0);

            _fileSystemWatcher.EnableRaisingEvents = false;
            _fileSystemWatcher.Created -= FileSystemWatcher_Created;

            SaveDocument();

            ChangeStatus(ServiceActivity.Stopped);

            _logger.Info("The file watcher has stopped.");
        }

        public void ChangePageTimeout(int newTimeout)
        {
            _timeout = newTimeout * 1000;
            _savingTimer.Change(_timeout, _timeout);
        }

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

        private void SaveDocument()
        {
            ChangeStatus(ServiceActivity.Saving);

            try
            {
                var sequence = Interlocked.Exchange(ref _sequenceFileNames, new ConcurrentBag<string>());
                var destinationPath = _pdfService.SaveDocument(sequence.Reverse(), GenerateUniqueFileName());

                if (destinationPath != null)
                {
                    DocumentSaved?.Invoke(this, new DocumentEventArgs(destinationPath));
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                throw;
            }

            ChangeStatus(ServiceActivity.Waiting);
        }

        private string GenerateUniqueFileName()
        {
            return Guid.NewGuid().ToString();
        }

        private void TimerCallback(object target)
        {
            _logger.Debug("The next page has timed out.");
            SaveDocument();
        }

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

        private void ChangeStatus(ServiceActivity status)
        {
            _currentActivity = status;
        }
    }
}
