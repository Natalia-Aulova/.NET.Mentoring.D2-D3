using System;
using NLog;
using Topshelf;
using WindowsService.FileHandler.Interfaces;

namespace WindowsService.FileHandler
{
    internal class FileHandlerService : ServiceControl
    {
        private readonly IFileWatcherFactory _watcherFactory;
        private readonly ISettingsProvider _settingsProvider;
        private readonly ILogger _logger;
        private IFileWatcher[] _fileWatchers;
        
        public FileHandlerService(IFileWatcherFactory watcherFactory, ISettingsProvider settingsProvider, ILogger logger)
        {
            _watcherFactory = watcherFactory;
            _settingsProvider = settingsProvider;
            _logger = logger;
        }

        public bool Start(HostControl hostControl)
        {
            _logger.Info("The file handler service is starting.");

            var sourceFolderPaths = _settingsProvider
                .GetSetting("SourceFolderPaths")
                .Split(new[] { "|" }, StringSplitOptions.RemoveEmptyEntries);

            var destinationFolderPath = _settingsProvider.GetSetting("DestinationFolderPath");
            var timeout = int.Parse(_settingsProvider.GetSetting("PageTimeout")) * 1000;

            _fileWatchers = new IFileWatcher[sourceFolderPaths.Length];

            try
            {
                for (int i = 0; i < sourceFolderPaths.Length; i++)
                {
                    var watcher = _watcherFactory.GetWatcher();
                    watcher.Start(sourceFolderPaths[i], destinationFolderPath, timeout);
                    _fileWatchers[i] = watcher;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
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
                for (int i = 0; i < _fileWatchers.Length; i++)
                {
                    _fileWatchers[i].Stop();
                }

                _fileWatchers = null;
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
