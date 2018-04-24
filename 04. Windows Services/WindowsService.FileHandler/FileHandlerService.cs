using System;
using System.Collections.Generic;
using System.Linq;
using NLog;
using Topshelf;
using WindowsService.FileHandler.Interfaces;

namespace WindowsService.FileHandler
{
    internal class FileHandlerService : ServiceControl
    {
        private readonly IFileHandlerFactory _fileHandlerFactory;
        private readonly ISettingsProvider _settingsProvider;
        private readonly ILogger _logger;
        private List<IFileHandler> _fileHandlers;
        
        public FileHandlerService(IFileHandlerFactory handlerFactory, ISettingsProvider settingsProvider, ILogger logger)
        {
            _fileHandlerFactory = handlerFactory;
            _settingsProvider = settingsProvider;
            _logger = logger;
            _fileHandlers = new List<IFileHandler>();
        }

        public bool Start(HostControl hostControl)
        {
            _logger.Info("The file handler service is starting.");

            var sourceFolderPaths = _settingsProvider.GetSourceFolderPaths();
            var timeout = _settingsProvider.GetPageTimeout() * 1000;

            try
            {
                _fileHandlers = sourceFolderPaths.Select(sourceFolderPath =>
                {
                    var handler = _fileHandlerFactory.GetHandler();
                    handler.Start(sourceFolderPath, timeout);
                    return handler;
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                throw;
            }

            _logger.Info("The file handler service has started.");

            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            _logger.Info("The file handler service is stopping.");

            try
            {
                _fileHandlers.ForEach(handler => handler.Stop());
                _fileHandlers.Clear();
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                throw;
            }

            _logger.Info("The file handler service has stopped.");

            return true;
        }
    }
}
