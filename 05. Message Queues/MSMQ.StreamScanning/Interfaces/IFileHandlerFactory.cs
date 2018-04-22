namespace MSMQ.StreamScanning.Interfaces
{
    public interface IFileHandlerFactory
    {
        IFileHandler GetHandler(int handlerNumber);
    }
}
