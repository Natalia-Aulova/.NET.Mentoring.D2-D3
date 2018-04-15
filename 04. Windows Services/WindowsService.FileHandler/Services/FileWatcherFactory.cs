using WindowsService.FileHandler.Interfaces;

namespace WindowsService.FileHandler.Services
{
    public class FileWatcherFactory : IFileWatcherFactory
    {
        private readonly INameHelper _nameHelper;
        private readonly IPdfService _pdfService;

        public FileWatcherFactory(INameHelper nameHelper, IPdfService pdfService)
        {
            _nameHelper = nameHelper;
            _pdfService = pdfService;
        }

        public IFileWatcher GetWatcher()
        {
            return new FileWatcher(_nameHelper, _pdfService);
        }
    }
}
