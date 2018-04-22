using MSMQ.StreamScanning.CentralService.Interfaces;
using MSMQ.StreamScanning.Common.Interfaces;
using NLog;

namespace MSMQ.StreamScanning.CentralService.Services
{
    public class DownloadClientFactory : IDownloadClientFactory
    {
        private readonly ISettingsProvider _settingsProvider;

        public DownloadClientFactory(ISettingsProvider settingsProvider)
        {
            _settingsProvider = settingsProvider;
        }

        public IDownloadClient GetClient(string url)
        {
            var destinationFolder = _settingsProvider.GetSetting("DownloadDestinationFolder");
            return new DownloadClient(destinationFolder, url, LogManager.GetLogger(nameof(DownloadClient)));
        }
    }
}
