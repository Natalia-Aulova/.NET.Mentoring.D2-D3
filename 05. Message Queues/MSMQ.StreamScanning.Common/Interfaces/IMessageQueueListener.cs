using System;
using MSMQ.StreamScanning.Common.Models;

namespace MSMQ.StreamScanning.Common.Interfaces
{
    public interface IMessageQueueListener
    {
        event EventHandler<MessageEventArgs> MessageReceived;

        void Start();

        void Stop();
    }
}
