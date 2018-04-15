using PdfSharp.Pdf;
using System.IO;
using System.Threading;
using WindowsService.FileHandler.Interfaces;

namespace WindowsService.FileHandler.Services
{
    public class FileWatcher : IFileWatcher
    {
        private static object lockObject = new object();
        private volatile int _sequenceNumber = 0;
        private PdfDocument _pdfDocument;

        private readonly INameHelper _nameHelper;
        private readonly IPdfService _pdfService;

        private string _destinationFolderPath;
        private int _timeout;

        private FileSystemWatcher _fileSystemWatcher;
        private Timer _savingTimer;

        public FileWatcher(INameHelper nameHelper, IPdfService pdfService)
        {
            _nameHelper = nameHelper;
            _pdfService = pdfService;
        }

        public void Start(string sourceFolderPath, string destinationFolderPath, int saveTimeout)
        {
            _destinationFolderPath = destinationFolderPath;

            lock (lockObject)
            {
                _pdfDocument = _pdfService.CreateDocument();
            }

            _timeout = saveTimeout;
            _savingTimer = new Timer(TimerCallback);
            _savingTimer.Change(_timeout, _timeout);

            _fileSystemWatcher = new FileSystemWatcher();

            _fileSystemWatcher.Path = sourceFolderPath;
            _fileSystemWatcher.Created += FileSystemWatcher_Created;
            _fileSystemWatcher.IncludeSubdirectories = false;
            _fileSystemWatcher.EnableRaisingEvents = true;

            Initialize(sourceFolderPath);
        }

        public void Stop()
        {
            _savingTimer.Change(Timeout.Infinite, 0);

            _fileSystemWatcher.EnableRaisingEvents = false;
            _fileSystemWatcher.Created -= FileSystemWatcher_Created;

            lock (lockObject)
            {
                _pdfService.SaveDocument(Path.Combine(_destinationFolderPath, _nameHelper.GenerateUniqueFileName("pdf")), _pdfDocument);
            }
        }

        private void FileSystemWatcher_Created(object sender, FileSystemEventArgs e)
        {
            if (!File.Exists(e.FullPath) || !_nameHelper.IsNameMatch(e.Name))
            {
                return;
            }

            var number = _nameHelper.GetFileNameNumber(e.Name);

            if (number - _sequenceNumber > 1)
            {
                StartNewDocument();
            }

            lock (lockObject)
            {
                _pdfService.AddImage(e.FullPath, _pdfDocument);
                _sequenceNumber = number;
            }

            _savingTimer.Change(_timeout, _timeout);
        }

        private void StartNewDocument()
        {
            lock (lockObject)
            {
                _pdfService.SaveDocument(Path.Combine(_destinationFolderPath, _nameHelper.GenerateUniqueFileName("pdf")), _pdfDocument);
                _pdfDocument = _pdfService.CreateDocument();
            }
        }

        private void TimerCallback(object target)
        {
            StartNewDocument();
            _savingTimer.Change(_timeout, _timeout);
        }

        private void Initialize(string sourceFolderPath)
        {
            foreach (var file in Directory.GetFiles(sourceFolderPath))
            {
                var directory = Path.GetDirectoryName(file);
                var fileName = Path.GetFileName(file);
                var eventArgs = new FileSystemEventArgs(
                    WatcherChangeTypes.Created,
                    Path.GetDirectoryName(file),
                    Path.GetFileName(file));

                FileSystemWatcher_Created(this, eventArgs);
            }
        }
    }
}
