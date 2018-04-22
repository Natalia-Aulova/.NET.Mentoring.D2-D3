using System;
using MSMQ.StreamScanning.Common.Interfaces;

namespace MSMQ.StreamScanning.Common.Services
{
    public class MessageQueueFactory : IMessageQueueFactory
    {
        public IMessageQueueListener GetListener(string queuePath, Type[] types)
        {
            return new MessageQueueListener(queuePath, types);
        }

        public IMessageQueueSender GetSender(string queuePath)
        {
            return new MessageQueueSender(queuePath);
        }
    }
}
