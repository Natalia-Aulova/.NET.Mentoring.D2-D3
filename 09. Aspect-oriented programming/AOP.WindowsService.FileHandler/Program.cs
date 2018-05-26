using AOP.WindowsService.FileHandler.Helpers;
using AOP.WindowsService.FileHandler.Providers;
using AOP.WindowsService.FileHandler.Services;
using NLog;
using Topshelf;

namespace AOP.WindowsService.FileHandler
{
    public class Program
    {
        private static void Main(string[] args)
        {
            var settingsProvider = new SettingsProvider();
            var nameHelper = new FileNameHelper(settingsProvider);
            var pdfService = new PdfService(settingsProvider, LogManager.GetLogger(nameof(PdfService)));
            var watcherFactory = new FileHandlerFactory(nameHelper, pdfService);

            HostFactory.Run(x =>
            {
                x.Service(() => new FileHandlerService(watcherFactory, settingsProvider, LogManager.GetLogger(nameof(FileHandlerService))));
                x.SetServiceName("WindowsService.FileHandlerService");
                x.SetDisplayName("File Handler Service");
                x.StartAutomaticallyDelayed();
                x.RunAsLocalService();
                x.EnableServiceRecovery(y => y.RestartService(1).RestartService(1));
            });
        }
    }
}
