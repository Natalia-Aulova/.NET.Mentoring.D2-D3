using System;
using System.Messaging;
using MSMQ.StreamScanning.Common.Interfaces;
using MSMQ.StreamScanning.Common.Models;
using MSMQ.StreamScanning.Common.Helpers;

namespace MSMQ.StreamScanning.Common.Services
{
    public class MessageQueueListener : IMessageQueueListener
    {
        private bool _listeningEnabled;
        private MessageQueue _queue;

        public event EventHandler<MessageEventArgs> MessageReceived;
        
        public MessageQueueListener(string queuePath, Type[] types)
        {
            _queue = MessageQueueHelper.GetOrCreateQueue(queuePath);

            if (types != null && types.Length > 0)
            {
                _queue.Formatter = new XmlMessageFormatter(types);
            }
        }

        public void Start()
        {
            _listeningEnabled = true;
            _queue.ReceiveCompleted += OnReceiveCompleted;

            StartListening();
        }

        public void Stop()
        {
            _listeningEnabled = false;
            _queue.ReceiveCompleted -= OnReceiveCompleted;
        }

        private void StartListening()
        {
            if (_listeningEnabled)
            {
                _queue.BeginReceive();
            }
        }
        
        private void OnReceiveCompleted(object sender, ReceiveCompletedEventArgs e)
        {
            var message = _queue.EndReceive(e.AsyncResult);
            
            StartListening();
            MessageReceived?.Invoke(this, new MessageEventArgs(message.Body));
        }
    }
}
