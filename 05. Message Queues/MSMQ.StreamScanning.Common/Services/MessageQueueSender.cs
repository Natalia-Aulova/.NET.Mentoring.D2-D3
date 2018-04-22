using System.Messaging;
using MSMQ.StreamScanning.Common.Helpers;
using MSMQ.StreamScanning.Common.Interfaces;

namespace MSMQ.StreamScanning.Common.Services
{
    public class MessageQueueSender : IMessageQueueSender
    {
        private MessageQueue _queue;

        public MessageQueueSender(string queuePath)
        {
            _queue = MessageQueueHelper.GetOrCreateQueue(queuePath);
        }

        public void Send(object message)
        {
            _queue.Send(message);
        }
    }
}
