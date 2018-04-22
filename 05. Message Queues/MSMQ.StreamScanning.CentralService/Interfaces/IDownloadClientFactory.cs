namespace MSMQ.StreamScanning.CentralService.Interfaces
{
    public interface IDownloadClientFactory
    {
        IDownloadClient GetClient(string url);
    }
}
