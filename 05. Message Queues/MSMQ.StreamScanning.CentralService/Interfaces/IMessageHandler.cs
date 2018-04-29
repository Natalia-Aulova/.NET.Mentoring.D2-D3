using System;

namespace MSMQ.StreamScanning.CentralService.Interfaces
{
    public interface IMessageHandler
    {
        Type MessageType { get; }

        bool CanHandle(object message);

        void Handle(object message);

        void Stop();
    }
}
