using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using NLog;
using PdfSharp.Pdf;
using WindowsService.FileHandler.Interfaces;

namespace WindowsService.FileHandler.Services
{
    public class FileWatcher : IFileWatcher
    {
        private static object lockObject = new object();

        private readonly CancellationTokenSource _tokenSource;

        private volatile int _sequenceNumber = 0;
        private PdfDocument _pdfDocument;

        private readonly INameHelper _nameHelper;
        private readonly IPdfService _pdfService;
        private readonly ILogger _logger;

        private string _destinationFolderPath;
        private int _timeout;

        private FileSystemWatcher _fileSystemWatcher;
        private Timer _savingTimer;

        public FileWatcher(INameHelper nameHelper, IPdfService pdfService, ILogger logger)
        {
            _tokenSource = new CancellationTokenSource();
            _nameHelper = nameHelper;
            _pdfService = pdfService;
            _logger = logger;
            _savingTimer = new Timer(TimerCallback);
            _fileSystemWatcher = new FileSystemWatcher();
            _fileSystemWatcher.IncludeSubdirectories = false;
        }

        public async void Start(string sourceFolderPath, string destinationFolderPath, int saveTimeout)
        {
            _logger.Info("The file watcher is starting.");

            _destinationFolderPath = destinationFolderPath;

            lock (lockObject)
            {
                _pdfDocument = _pdfService.CreateDocument();
            }

            _timeout = saveTimeout;
            _savingTimer.Change(_timeout, _timeout);

            _fileSystemWatcher.Path = sourceFolderPath;
            _fileSystemWatcher.Created += FileSystemWatcher_Created;
            _fileSystemWatcher.EnableRaisingEvents = true;

            try
            {
                await Initialize(sourceFolderPath, _tokenSource.Token);
            }
            catch (OperationCanceledException ex)
            {
                _logger.Warn(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);
                throw;
            }

            _logger.Info("The file watcher has started.");
        }

        public void Stop()
        {
            _logger.Info("The file watcher is stopping.");

            _tokenSource.Cancel();

            _savingTimer.Change(Timeout.Infinite, 0);

            _fileSystemWatcher.EnableRaisingEvents = false;
            _fileSystemWatcher.Created -= FileSystemWatcher_Created;

            lock (lockObject)
            {
                _pdfService.SaveDocument(GetDestinationPath(), _pdfDocument);
            }

            _logger.Info("The file watcher has stopped.");
        }

        private void FileSystemWatcher_Created(object sender, FileSystemEventArgs e)
        {
            if (_tokenSource.IsCancellationRequested)
            {
                return;
            }

            if (!File.Exists(e.FullPath) || !_nameHelper.IsNameMatch(e.Name))
            {
                return;
            }

            var number = _nameHelper.GetFileNameNumber(e.Name);

            if (number - _sequenceNumber != 1)
            {
                _logger.Debug(@"""Jumping"" numbering has been detected. Starting new document.");
                StartNewDocument();
            }

            lock (lockObject)
            {
                _logger.Debug("Adding new file to the document.");
                _pdfService.AddImage(e.FullPath, _pdfDocument);
                _sequenceNumber = number;
            }

            _savingTimer.Change(_timeout, _timeout);
        }

        private void StartNewDocument()
        {
            lock (lockObject)
            {
                _pdfService.SaveDocument(GetDestinationPath(), _pdfDocument);
                _pdfDocument = _pdfService.CreateDocument();
            }
        }

        private void TimerCallback(object target)
        {
            _logger.Debug("The next page has timed out. Starting new document.");
            StartNewDocument();
        }

        private async Task Initialize(string sourceFolderPath, CancellationToken token)
        {
            await Task.Factory.StartNew(() =>
            {
                foreach (var file in Directory.GetFiles(sourceFolderPath))
                {
                    token.ThrowIfCancellationRequested();

                    var eventArgs = new FileSystemEventArgs(
                        WatcherChangeTypes.Created,
                        Path.GetDirectoryName(file),
                        Path.GetFileName(file));

                    FileSystemWatcher_Created(this, eventArgs);
                }
            }, token);
        }

        private string GetDestinationPath()
        {
            return Path.Combine(_destinationFolderPath, _nameHelper.GenerateUniqueFileName("pdf"));
        }
    }
}
