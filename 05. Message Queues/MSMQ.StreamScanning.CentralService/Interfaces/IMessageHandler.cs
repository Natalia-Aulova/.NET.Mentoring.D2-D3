namespace MSMQ.StreamScanning.CentralService.Interfaces
{
    public interface IMessageHandler
    {
        bool CanHandle(object message);

        void Handle(object message);

        void Stop();
    }
}
