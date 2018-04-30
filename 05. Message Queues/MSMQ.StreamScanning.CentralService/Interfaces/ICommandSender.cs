namespace MSMQ.StreamScanning.CentralService.Interfaces
{
    public interface ICommandSender
    {
        void Start();

        void Stop();

        void AddSubscriber(string queuePath);
    }
}
