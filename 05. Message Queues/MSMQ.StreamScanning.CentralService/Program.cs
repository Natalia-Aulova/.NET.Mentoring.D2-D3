using MSMQ.StreamScanning.CentralService.Interfaces;
using MSMQ.StreamScanning.CentralService.Services;
using MSMQ.StreamScanning.Common.Providers;
using MSMQ.StreamScanning.Common.Services;
using NLog;
using Topshelf;

namespace MSMQ.StreamScanning.CentralService
{
    public class Program
    {
        private static void Main(string[] args)
        {
            var settingsProvider = new SettingsProvider();
            var msmqFactory = new MessageQueueFactory();
            var downloadFactory = new DownloadClientFactory(settingsProvider);
            var logger = LogManager.GetLogger(nameof(FileControlService));

            var messageHandlers = new IMessageHandler[]
            {
                new FileInfoMessageHandler(downloadFactory, LogManager.GetLogger(nameof(FileInfoMessageHandler))),
                new ServiceInfoMessageHandler(settingsProvider, LogManager.GetLogger(nameof(ServiceInfoMessageHandler)))
            };

            HostFactory.Run(x =>
            {
                x.Service(() => new FileControlService(msmqFactory, settingsProvider, logger, messageHandlers));
                x.SetServiceName("MSMQ.StreamScanning.CentralService");
                x.SetDisplayName("MSMQ StreamScanning Central Service");
                x.StartAutomaticallyDelayed();
                x.RunAsLocalService();
                x.EnableServiceRecovery(y => y.RestartService(1).RestartService(1));
            });
        }
    }
}
