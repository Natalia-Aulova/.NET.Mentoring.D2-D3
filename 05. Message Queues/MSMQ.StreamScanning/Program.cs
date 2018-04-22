using MSMQ.StreamScanning.Common.Providers;
using MSMQ.StreamScanning.Common.Services;
using MSMQ.StreamScanning.Helpers;
using MSMQ.StreamScanning.Services;
using NLog;
using Topshelf;

namespace MSMQ.StreamScanning
{
    public class Program
    {
        private static void Main(string[] args)
        {
            var settingsProvider = new SettingsProvider();
            var nameHelper = new FileNameHelper(settingsProvider);
            var pdfService = new PdfService(settingsProvider, LogManager.GetLogger(nameof(PdfService)));
            var watcherFactory = new FileHandlerFactory(nameHelper, pdfService);
            var msmqFactory = new MessageQueueFactory();
            var logger = LogManager.GetLogger(nameof(FileHandlerService));

            HostFactory.Run(x =>
            {
                x.Service(() => new FileHandlerService(watcherFactory, msmqFactory, settingsProvider, logger));
                x.SetServiceName("MSMQ.StreamScanning.Worker");
                x.SetDisplayName("MSMQ StreamScanning Worker");
                x.StartAutomaticallyDelayed();
                x.RunAsLocalService();
                x.EnableServiceRecovery(y => y.RestartService(1).RestartService(1));
            });
        }
    }
}
