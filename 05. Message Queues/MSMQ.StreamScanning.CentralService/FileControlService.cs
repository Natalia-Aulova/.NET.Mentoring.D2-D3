using System;
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
        private readonly ICommandSender _commandSender;
        private readonly IMessageHandler[] _messageHandlers;
        private readonly Type[] _messageTypes;

        private IMessageQueueListener _msmqListener;

        public FileControlService(
            IMessageQueueFactory msmqFactory, 
            ISettingsProvider settingsProvider, 
            ILogger logger, 
            ICommandSender commandSender, 
            IMessageHandler[] messageHandlers)
        {
            _msmqFactory = msmqFactory;
            _settingsProvider = settingsProvider;
            _logger = logger;
            _commandSender = commandSender;
            _messageHandlers = messageHandlers;
            _messageTypes = _messageHandlers
                .Select(x => x.MessageType)
                .Union(new[] { typeof(AddSubscriberMessage) })
                .ToArray();
        }

        public bool Start(HostControl hostControl)
        {
            _logger.Info("The file control service is starting.");

            var queueName = _settingsProvider.GetMessageQueueName();
            var queuePath = MessageQueueHelper.GetQueuePath(queueName);

            _msmqListener = _msmqFactory.GetListener(queuePath, _messageTypes);
            _msmqListener.MessageReceived += OnMessageReceived;
            _msmqListener.Start();
            _commandSender.Start();

            _logger.Info("The file control service has started.");

            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            _logger.Info("The file control service is stopping.");

            _msmqListener.MessageReceived -= OnMessageReceived;
            _msmqListener.Stop();
            
            foreach(var handler in _messageHandlers)
            {
                handler.Stop();
            }

            _commandSender.Stop();

            _logger.Info("The file control service has stopped.");

            return true;
        }

        private void OnMessageReceived(object sender, MessageEventArgs e)
        {
            var message = e.MessageBody;

            if (message is AddSubscriberMessage)
            {
                var subscriber = ((AddSubscriberMessage)message).SubscriberQueue;
                _logger.Info($"Adding the subscriber: {subscriber}");
                _commandSender.AddSubscriber(subscriber);
                return;
            }

            var handler = _messageHandlers.FirstOrDefault(x => x.CanHandle(message));

            if (handler == null)
            {
                _logger.Warn($"Unable to process the message of the type {message.GetType()}");
                return;
            }

            handler.Handle(message);
        }
    }
}
