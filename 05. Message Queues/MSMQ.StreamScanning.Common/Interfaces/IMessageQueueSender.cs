namespace MSMQ.StreamScanning.Common.Interfaces
{
    public interface IMessageQueueSender
    {
        void Send(object message);
    }
}
