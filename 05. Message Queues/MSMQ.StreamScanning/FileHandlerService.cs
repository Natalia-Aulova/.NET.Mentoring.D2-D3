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
        private IMessageQueueListener _msmqListener;
        private Timer _reportingTimer;
        private int _pageTimeout;

        public FileHandlerService(IFileHandlerFactory handlerFactory, IMessageQueueFactory msmqFactory, ISettingsProvider settingsProvider, ILogger logger)
        {
            _fileHandlerFactory = handlerFactory;
            _msmqFactory = msmqFactory;
            _settingsProvider = settingsProvider;
            _logger = logger;
            _fileHandlers = new List<IFileHandler>();
            _reportingTimer = new Timer(ReportStatus);
        }

        public bool Start(HostControl hostControl)
        {
            _logger.Info("The file handler service is starting.");

            var queueName = _settingsProvider.GetMessageQueueName();
            var queuePath = MessageQueueHelper.GetQueuePath(queueName);

            InitializeCentralQueueSender(queuePath);
            InitializeQueueListener(queuePath);

            var sourceFolderPaths = _settingsProvider.GetSourceFolderPaths();
            _pageTimeout = _settingsProvider.GetPageTimeout();

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

            var reportingTimeout = _settingsProvider.GetReportingTimeout() * 1000;
            _reportingTimer.Change(reportingTimeout, reportingTimeout);

            _logger.Info("The file handler service has started.");

            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            _logger.Info("The file handler service is stopping.");

            _msmqListener.Stop();
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

        private void OnMessageReceived(object sender, MessageEventArgs e)
        {
            if (e.MessageBody is UpdateStatusMessage)
            {
                _logger.Info($"The command {nameof(UpdateStatusMessage)} has been received.");
                ReportStatus(null);
                return;
            }

            if (e.MessageBody is UpdatePageTimeoutMessage)
            {
                _pageTimeout = ((UpdatePageTimeoutMessage)e.MessageBody).Timeout;
                _logger.Info($"The command {nameof(UpdatePageTimeoutMessage)} has been received. New page timeout: {_pageTimeout} seconds.");
                _fileHandlers.ForEach(handler => handler.ChangePageTimeout(_pageTimeout));
                return;
            }

            _logger.Warn($"Unable to process the message of the type {e.MessageBody.GetType()}");
        }

        private void ReportStatus(object target)
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

        private void InitializeCentralQueueSender(string currentQueuePath)
        {
            var centralQueueName = _settingsProvider.GetCentralMessageQueueName();
            var centralQueueMachine = _settingsProvider.GetCentralMessageQueueMachine();
            var centralQueuePath = MessageQueueHelper.GetQueuePath(centralQueueName, centralQueueMachine);

            _msmqSender = _msmqFactory.GetSender(centralQueuePath);
            _msmqSender.Send(new AddSubscriberMessage
            {
                SubscriberQueue = currentQueuePath
            });
        }

        private void InitializeQueueListener(string queuePath)
        {
            _msmqListener = _msmqFactory.GetListener(queuePath, new[]
            {
                typeof(UpdateStatusMessage),
                typeof(UpdatePageTimeoutMessage)
            });

            _msmqListener.MessageReceived += OnMessageReceived;
            _msmqListener.Start();
        }
    }
}
