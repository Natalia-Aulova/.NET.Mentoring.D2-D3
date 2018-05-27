using AOP.WindowsService.FileHandler.Helpers;
using AOP.WindowsService.FileHandler.Infrastructure;
using AOP.WindowsService.FileHandler.Interfaces;
using AOP.WindowsService.FileHandler.Providers;
using AOP.WindowsService.FileHandler.Services;
using Castle.DynamicProxy;
using NLog;
using Topshelf;

namespace AOP.WindowsService.FileHandler
{
    public class Program
    {
        private static void Main(string[] args)
        {
            HostFactory.Run(x =>
            {
                x.Service(GetFileHandlerService);
                x.SetServiceName("WindowsService.FileHandlerService");
                x.SetDisplayName("File Handler Service");
                x.StartAutomaticallyDelayed();
                x.RunAsLocalService();
                x.EnableServiceRecovery(y => y.RestartService(1).RestartService(1));
            });
        }

        private static ServiceControl GetFileHandlerService()
        {
            var loggingInterceptor = new LoggingInterceptor(new AspectLogger(LogManager.GetLogger(nameof(AspectLogger))));

            var generator = new ProxyGenerator();

            var settingsProvider = generator.CreateInterfaceProxyWithTarget<ISettingsProvider>(
                new SettingsProvider(), 
                loggingInterceptor);

            var nameHelper = generator.CreateInterfaceProxyWithTarget<INameHelper>(
                new FileNameHelper(settingsProvider), 
                loggingInterceptor);

            var pdfService = generator.CreateInterfaceProxyWithTarget<IPdfService>(
                new PdfService(settingsProvider, LogManager.GetLogger(nameof(PdfService))),
                loggingInterceptor);

            var watcherFactory = generator.CreateInterfaceProxyWithTarget<IFileHandlerFactory>(
                new FileHandlerFactory(nameHelper, pdfService),
                loggingInterceptor);

            var fileHandlerService = generator.CreateInterfaceProxyWithTarget<ServiceControl>(
                new FileHandlerService(watcherFactory, settingsProvider, LogManager.GetLogger(nameof(FileHandlerService))),
                loggingInterceptor);

            return fileHandlerService;
        }
    }
}
