using System.Threading;
using System.Threading.Tasks;

namespace AsyncAwait.WPF.Services
{
    public interface IDownloader
    {
        Task<byte[]> Load(string uri, CancellationToken token);
    }
}
