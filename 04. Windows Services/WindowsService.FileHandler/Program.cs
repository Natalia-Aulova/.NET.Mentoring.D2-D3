using Topshelf;
using WindowsService.FileHandler.Helpers;
using WindowsService.FileHandler.Providers;
using WindowsService.FileHandler.Services;

namespace WindowsService.FileHandler
{
    public class Program
    {
        private static void Main(string[] args)
        {
            var settingsProvider = new SettingsProvider();
            var nameHelper = new FileNameHelper(settingsProvider);
            var pdfService = new PdfService();
            var watcherFactory = new FileWatcherFactory(nameHelper, pdfService);

            HostFactory.Run(x =>
            {
                x.Service(() => new FileHandlerService(watcherFactory, settingsProvider));
                x.SetServiceName("WindowsService.FileHandlerService");
                x.SetDisplayName("File Handler Service");
                x.StartAutomaticallyDelayed();
                x.RunAsLocalService();
                x.EnableServiceRecovery(y => y.RestartService(1).RestartService(1));
            });
        }
    }
}
