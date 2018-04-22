using System;

namespace MSMQ.StreamScanning.Common.Models
{
    public class AddSubscriberMessage
    {
        public string SubscriberQueue { get; set; }

        public Type MessageType { get; set; }
    }
}
