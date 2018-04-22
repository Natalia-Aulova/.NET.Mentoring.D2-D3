using System;
using System.Threading.Tasks;

namespace MSMQ.StreamScanning.CentralService.Interfaces
{
    public interface IDownloadClient : IDisposable
    {
        Task Download();

        void Cancel();
    }
}
