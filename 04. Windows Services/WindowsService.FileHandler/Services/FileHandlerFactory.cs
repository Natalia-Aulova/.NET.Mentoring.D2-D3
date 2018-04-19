using System;
using NLog;
using WindowsService.FileHandler.Interfaces;

namespace WindowsService.FileHandler.Services
{
    public class FileHandlerFactory : IFileHandlerFactory
    {
        private readonly INameHelper _nameHelper;
        private readonly IPdfService _pdfService;

        public FileHandlerFactory(INameHelper nameHelper, IPdfService pdfService)
        {
            _nameHelper = nameHelper;
            _pdfService = pdfService;
        }

        public IFileHandler GetHandler(int loggerNumber)
        {
            var loggerName = $"{nameof(FileHandler)} {loggerNumber}";
            return new FileHandler(_nameHelper, _pdfService, LogManager.GetLogger(loggerName));
        }
    }
}
