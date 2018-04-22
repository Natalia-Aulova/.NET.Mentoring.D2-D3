using System;
using MSMQ.StreamScanning.Common.Helpers;
using MSMQ.StreamScanning.Common.Interfaces;
using MSMQ.StreamScanning.Common.Models;
using MSMQ.StreamScanning.Interfaces;
using MSMQ.StreamScanning.Models;
using NLog;
using Topshelf;

namespace MSMQ.StreamScanning
{
    internal class FileHandlerService : ServiceControl
    {
        private readonly IFileHandlerFactory _fileHandlerFactory;
        private readonly IMessageQueueFactory _msmqFactory;
        private readonly ISettingsProvider _settingsProvider;
        private readonly ILogger _logger;

        private IFileHandler[] _fileHandlers;
        private IMessageQueueSender _msmqSender;

        public FileHandlerService(IFileHandlerFactory handlerFactory, IMessageQueueFactory msmqFactory, ISettingsProvider settingsProvider, ILogger logger)
        {
            _fileHandlerFactory = handlerFactory;
            _msmqFactory = msmqFactory;
            _settingsProvider = settingsProvider;
            _logger = logger;
        }

        public bool Start(HostControl hostControl)
        {
            _logger.Info("The file handler service is starting.");

            var centralQueueName = _settingsProvider.GetSetting("CentralMessageQueueName");
            var centralQueueMachine = _settingsProvider.GetSetting("CentralMessageQueueMachine");
            var centralQueuePath = MessageQueueHelper.GetQueuePath(centralQueueName, centralQueueMachine);
            _msmqSender = _msmqFactory.GetSender(centralQueuePath);

            var sourceFolderPaths = _settingsProvider
                .GetSetting("SourceFolderPaths")
                .Split(new[] { "|" }, StringSplitOptions.RemoveEmptyEntries);

            var destinationFolderPath = _settingsProvider.GetSetting("DestinationFolderPath");
            var timeout = int.Parse(_settingsProvider.GetSetting("PageTimeout")) * 1000;

            _fileHandlers = new IFileHandler[sourceFolderPaths.Length];

            try
            {
                for (int i = 0; i < sourceFolderPaths.Length; i++)
                {
                    var handler = _fileHandlerFactory.GetHandler(i);
                    handler.DocumentSaved += OnDocumentSaved;
                    handler.Start(sourceFolderPaths[i], destinationFolderPath, timeout);
                    _fileHandlers[i] = handler;
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
                for (int i = 0; i < _fileHandlers.Length; i++)
                {
                    _fileHandlers[i].DocumentSaved -= OnDocumentSaved;
                    _fileHandlers[i].Stop();
                }

                _fileHandlers = null;
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                throw;
            }

            _logger.Info("The file handler service has stopped.");

            return true;
        }

        private void OnDocumentSaved(object sender, DocumentEventArgs e)
        {
            _msmqSender.Send(new FileInfoMessage
            {
                FilePath = e.FilePath
            });
        }
    }
}
