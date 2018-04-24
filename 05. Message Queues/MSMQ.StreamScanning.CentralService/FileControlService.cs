using System.Linq;
using MSMQ.StreamScanning.CentralService.Interfaces;
using MSMQ.StreamScanning.Common.Helpers;
using MSMQ.StreamScanning.Common.Interfaces;
using MSMQ.StreamScanning.Common.Models;
using NLog;
using Topshelf;

namespace MSMQ.StreamScanning.CentralService
{
    internal class FileControlService : ServiceControl
    {
        private readonly IMessageQueueFactory _msmqFactory;
        private readonly ISettingsProvider _settingsProvider;
        private readonly ILogger _logger;
        private readonly IMessageHandler[] _messageHandlers;

        private IMessageQueueListener _msmqListener;

        public FileControlService(IMessageQueueFactory msmqFactory, ISettingsProvider settingsProvider, ILogger logger, IMessageHandler[] messageHandlers)
        {
            _msmqFactory = msmqFactory;
            _settingsProvider = settingsProvider;
            _logger = logger;
            _messageHandlers = messageHandlers;
        }

        public bool Start(HostControl hostControl)
        {
            _logger.Info("The file control service is starting.");

            var queueName = _settingsProvider.GetMessageQueueName();
            var queuePath = MessageQueueHelper.GetQueuePath(queueName);

            var messageTypes = new[]
            {
                typeof(AddSubscriberMessage),
                typeof(FileInfoMessage),
                typeof(ServiceInfoMessage)
            };

            _msmqListener = _msmqFactory.GetListener(queuePath, messageTypes);
            _msmqListener.MessageReceived += OnMessageReceived;
            _msmqListener.Start();

            _logger.Info("The file control service has started.");

            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            _logger.Info("The file control service is stopping.");

            _msmqListener.Stop();
            
            foreach(var handler in _messageHandlers)
            {
                handler.Stop();
            }

            _logger.Info("The file control service has stopped.");

            return true;
        }

        private void OnMessageReceived(object sender, MessageEventArgs e)
        {
            var message = e.MessageBody;

            var handler = _messageHandlers.FirstOrDefault(x => x.CanHandle(message));

            if (handler == null)
            {
                _logger.Warn($"Unable to process the message of the type {e.GetType()}");
                return;
            }

            handler.Handle(message);
        }
    }
}
