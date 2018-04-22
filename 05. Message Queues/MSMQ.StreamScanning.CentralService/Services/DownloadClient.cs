using System.IO;
using System.Net;
using System.Threading.Tasks;
using MSMQ.StreamScanning.CentralService.Interfaces;
using NLog;

namespace MSMQ.StreamScanning.CentralService.Services
{
    public class DownloadClient : IDownloadClient
    {
        private readonly WebClient _client;
        private readonly string _url;
        private readonly string _destinationFolder;
        private readonly ILogger _logger;

        public DownloadClient(string destinationFolder, string url, ILogger logger)
        {
            _client = new WebClient();
            _url = url;
            _destinationFolder = destinationFolder;
            _logger = logger;
        }

        public async Task Download()
        {
            if (!Directory.Exists(_destinationFolder))
            {
                _logger.Info($"Creating destination folder {_destinationFolder}");
                Directory.CreateDirectory(_destinationFolder);
            }

            var fileName = Path.GetFileName(_url);
            var destinationPath = Path.Combine(_destinationFolder, fileName);

            _logger.Info($"Starting downloading the file: {_url}");

            await _client.DownloadFileTaskAsync(_url, destinationPath);
        }

        public void Cancel()
        {
            _logger.Info($"Canceling downloading the file: {_url}");
            _client.CancelAsync();
        }

        public void Dispose()
        {
            _client.Dispose();
        }
    }
}
