using System;
using System.IO;
using MSMQ.StreamScanning.Common.Interfaces;
using MSMQ.StreamScanning.Common.Models;
using NLog;

namespace MSMQ.StreamScanning.CentralService.Services
{
    public class ServiceInfoMessageHandler : MessageHandler<ServiceInfoMessage>
    {
        private readonly string _statusFile;
        private readonly ILogger _logger;

        public ServiceInfoMessageHandler(ISettingsProvider settingsProvider, ILogger logger)
        {
            _statusFile = settingsProvider.GetInputServiceStatusesFile();
            _logger = logger;
        }

        public override void Stop() { }

        protected override void HandleMessage(ServiceInfoMessage message)
        {
            try
            {
                using (var writer = File.AppendText(_statusFile))
                {
                    writer.WriteLine($"{message.MachineName},{message.CurrentServiceActivity},{message.PageTimeout}");
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                throw;
            }
        }
    }
}
