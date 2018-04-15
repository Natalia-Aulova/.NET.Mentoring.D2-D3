using System;
using Topshelf;
using WindowsService.FileHandler.Interfaces;

namespace WindowsService.FileHandler
{
    internal class FileHandlerService : ServiceControl
    {
        private readonly IFileWatcherFactory _watcherFactory;
        private readonly ISettingsProvider _settingsProvider;
        private IFileWatcher[] _fileWatchers;
        
        public FileHandlerService(IFileWatcherFactory watcherFactory, ISettingsProvider settingsProvider)
        {
            _watcherFactory = watcherFactory;
            _settingsProvider = settingsProvider;
        }

        public bool Start(HostControl hostControl)
        {
            var sourceFolderPaths = _settingsProvider
                .GetSetting("SourceFolderPaths")
                .Split(new[] { "|" }, StringSplitOptions.RemoveEmptyEntries);

            var destinationFolderPath = _settingsProvider.GetSetting("DestinationFolderPath");
            var timeout = int.Parse(_settingsProvider.GetSetting("PageTimeout")) * 1000;

            _fileWatchers = new IFileWatcher[sourceFolderPaths.Length];

            for (int i = 0; i < sourceFolderPaths.Length; i++)
            {
                var watcher = _watcherFactory.GetWatcher();
                watcher.Start(sourceFolderPaths[i], destinationFolderPath, timeout);
                _fileWatchers[i] = watcher;
            }
            
            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            for (int i = 0; i < _fileWatchers.Length; i++)
            {
                _fileWatchers[i].Stop();
            }

            _fileWatchers = null;

            return true;
        }
    }
}
