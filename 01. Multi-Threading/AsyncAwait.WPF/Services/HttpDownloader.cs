using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncAwait.WPF.Services
{
    public class HttpDownloader : IDownloader
    {
        public async Task<byte[]> Load(string uri, CancellationToken token)
        {
            var client = new HttpClient();
            var response = await client.GetAsync(uri, token);

            return await response.Content.ReadAsByteArrayAsync();
        }
    }
}
