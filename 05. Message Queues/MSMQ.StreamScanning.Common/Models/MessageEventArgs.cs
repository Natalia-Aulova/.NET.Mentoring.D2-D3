using System;

namespace MSMQ.StreamScanning.Common.Models
{
    public class MessageEventArgs : EventArgs
    {
        public object MessageBody { get; }

        public MessageEventArgs(object body)
        {
            MessageBody = body;
        }
    }
}
