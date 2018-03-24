using System.Threading;
using System.Threading.Tasks;

namespace AsyncAwait.WPF.Entities
{
    public class DownloadTask
    {
        public int Id { get; }

        public Task<byte[]> Task { get; }

        public CancellationTokenSource TokenSource { get; }

        public DownloadTask(int id, Task<byte[]> task, CancellationTokenSource tokenSource)
        {
            Id = id;
            Task = task;
            TokenSource = tokenSource;
        }
    }
}
