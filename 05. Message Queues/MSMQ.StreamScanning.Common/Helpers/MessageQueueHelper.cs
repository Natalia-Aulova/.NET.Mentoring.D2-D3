using System;
using System.Messaging;

namespace MSMQ.StreamScanning.Common.Helpers
{
    public static class MessageQueueHelper
    {
        public static MessageQueue GetOrCreateQueue(string queuePath)
        {
            return MessageQueue.Exists(queuePath)
                ? new MessageQueue(queuePath)
                : MessageQueue.Create(queuePath);
        }

        public static string GetQueuePath(string queueName)
        {
            return GetQueuePath(queueName, null);
        }

        public static string GetQueuePath(string queueName, string machineName)
        {
            if (string.IsNullOrWhiteSpace(machineName))
            {
                return BuildQueuePath(queueName, Environment.MachineName);
            }

            return BuildQueuePath(queueName, machineName);
        }

        private static string BuildQueuePath(string queueName, string machineName)
        {
            return $@"{machineName}\private$\{queueName}";
        }
    }
}
