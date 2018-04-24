using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using NLog;
using WindowsService.FileHandler.Interfaces;

namespace WindowsService.FileHandler.Services
{
    public class FileHandler : IFileHandler
    {
        private readonly CancellationTokenSource _tokenSource;

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
            _tokenSource = new CancellationTokenSource();
            _nameHelper = nameHelper;
            _pdfService = pdfService;
            _logger = logger;
            _sequenceFileNames = new ConcurrentBag<string>();
            _savingTimer = new Timer(TimerCallback);
            _fileSystemWatcher = new FileSystemWatcher();
            _fileSystemWatcher.IncludeSubdirectories = false;
        }

        public async void Start(string sourceFolderPath, int saveTimeout)
        {
            _logger.Info("The file watcher is starting.");

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

        public async void Stop()
        {
            _logger.Info("The file watcher is stopping.");

            _tokenSource.Cancel();

            _savingTimer.Change(Timeout.Infinite, 0);

            _fileSystemWatcher.EnableRaisingEvents = false;
            _fileSystemWatcher.Created -= FileSystemWatcher_Created;

            await SaveDocument();

            _logger.Info("The file watcher has stopped.");
        }

        private async void FileSystemWatcher_Created(object sender, FileSystemEventArgs e)
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
                _logger.Debug(@"""Jumping"" numbering has been detected. Saving the sequence to a document.");
                await SaveDocument();
            }

            _logger.Debug("Adding new file to the sequence.");

            _sequenceFileNames.Add(e.FullPath);
            _sequenceNumber = number;

            _savingTimer.Change(_timeout, _timeout);
        }

        private async Task SaveDocument()
        {
            var sequence = Interlocked.Exchange(ref _sequenceFileNames, new ConcurrentBag<string>());

            try
            {
                await _pdfService.SaveDocument(sequence, GenerateUniqueFileName(), _tokenSource.Token);
            }
            catch (OperationCanceledException ex)
            {
                _logger.Warn(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                throw;
            }
        }

        private string GenerateUniqueFileName()
        {
            return Guid.NewGuid().ToString();
        }

        private async void TimerCallback(object target)
        {
            _logger.Debug("The next page has timed out.");
            await SaveDocument();
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
    }
}
