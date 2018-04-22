using System;

namespace MSMQ.StreamScanning.Common.Interfaces
{
    public interface IMessageQueueFactory
    {
        IMessageQueueListener GetListener(string queueName, Type[] types);

        IMessageQueueSender GetSender(string queuePath);
    }
}
