using MSMQ.StreamScanning.CentralService.Interfaces;

namespace MSMQ.StreamScanning.CentralService.Services
{
    public abstract class MessageHandler<T> : IMessageHandler
    {
        public bool CanHandle(object message)
        {
            return typeof(T) == message.GetType();
        }

        public void Handle(object message)
        {
            HandleMessage((T)message);
        }

        public abstract void Stop();

        protected abstract void HandleMessage(T message);
    }
}
