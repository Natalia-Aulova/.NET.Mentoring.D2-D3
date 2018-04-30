using System;
using MSMQ.StreamScanning.CentralService.Interfaces;

namespace MSMQ.StreamScanning.CentralService.Services
{
    public abstract class MessageHandler<T> : IMessageHandler
    {
        public Type MessageType => typeof(T);

        public bool CanHandle(object message)
        {
            return MessageType == message.GetType();
        }

        public void Handle(object message)
        {
            HandleMessage((T)message);
        }

        public virtual void Stop() { }

        protected abstract void HandleMessage(T message);
    }
}
