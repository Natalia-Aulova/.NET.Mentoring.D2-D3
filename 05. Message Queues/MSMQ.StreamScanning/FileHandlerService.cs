using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
        private List<IFileHandler> _fileHandlers;
        private IMessageQueueSender _msmqSender;
        private Timer _reportingTimer;
        private int _pageTimeout;

        public FileHandlerService(IFileHandlerFactory handlerFactory, IMessageQueueFactory msmqFactory, ISettingsProvider settingsProvider, ILogger logger)
        {
            _fileHandlerFactory = handlerFactory;
            _msmqFactory = msmqFactory;
            _settingsProvider = settingsProvider;
            _logger = logger;
            _fileHandlers = new List<IFileHandler>();
            _reportingTimer = new Timer(ReportingTimerCallback);
        }

        public bool Start(HostControl hostControl)
        {
            _logger.Info("The file handler service is starting.");

            var centralQueueName = _settingsProvider.GetCentralMessageQueueName();
            var centralQueueMachine = _settingsProvider.GetCentralMessageQueueName();
            var centralQueuePath = MessageQueueHelper.GetQueuePath(centralQueueName, centralQueueMachine);
            _msmqSender = _msmqFactory.GetSender(centralQueuePath);

            var sourceFolderPaths = _settingsProvider.GetSourceFolderPaths();
            _pageTimeout = _settingsProvider.GetPageTimeout() * 1000;
            var reportingTimeout = _settingsProvider.GetReportingTimeout() * 1000;

            try
            {
                _fileHandlers = sourceFolderPaths.Select(sourceFolderPath =>
                {
                    var handler = _fileHandlerFactory.GetHandler();
                    handler.DocumentSaved += OnDocumentSaved;
                    handler.Start(sourceFolderPath, _pageTimeout);
                    return handler;
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                throw;
            }

            _reportingTimer.Change(reportingTimeout, reportingTimeout);

            _logger.Info("The file handler service has started.");

            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            _logger.Info("The file handler service is stopping.");

            _reportingTimer.Change(Timeout.Infinite, 0);

            try
            {
                _fileHandlers.ForEach(handler => 
                {
                    handler.DocumentSaved -= OnDocumentSaved;
                    handler.Stop();
                });

                _fileHandlers.Clear();
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
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

        private void ReportingTimerCallback(object target)
        {
            _msmqSender.Send(new ServiceInfoMessage
            {
                MachineName = Environment.MachineName,
                CurrentServiceActivity = GetCurrentStatus(),
                PageTimeout = _pageTimeout
            });
        }

        private string GetCurrentStatus()
        {
            if (_fileHandlers.Any(x => x.CurrentActivity == ServiceActivity.Saving))
            {
                return ServiceActivityMessages.Saving;
            }

            var handlerStatus = _fileHandlers.FirstOrDefault()?.CurrentActivity;

            switch (handlerStatus)
            {
                case ServiceActivity.Starting:
                    return ServiceActivityMessages.Starting;
                case ServiceActivity.Stopping:
                    return ServiceActivityMessages.Stopping;
                default:
                    return ServiceActivityMessages.Waiting;
            }
        }
    }
}
