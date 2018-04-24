using System;
using System.Configuration;
using MSMQ.StreamScanning.Common.Interfaces;

namespace MSMQ.StreamScanning.Common.Providers
{
    public class SettingsProvider : ISettingsProvider
    {
        public string GetBrokenFolderName()
        {
            return Get("BrokenFolderName");
        }

        public string GetDestinationFolderPath()
        {
            return Get("DestinationFolderPath");
        }

        public string GetNameTemplate()
        {
            return Get("NameTemplate");
        }

        public int GetPageTimeout()
        {
            return int.Parse(Get("PageTimeout"));
        }

        public int GetRetryCount()
        {
            return int.Parse(Get("RetryCount"));
        }

        public int GetRetryDelay()
        {
            return int.Parse(Get("RetryDelay"));
        }

        public string[] GetSourceFolderPaths()
        {
            return Get("SourceFolderPaths").Split(new[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
        }

        public string[] GetSupportedExtensions()
        {
            return Get("SupportedExtensions").Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
        }

        public string GetMessageQueueName()
        {
            return Get("MessageQueueName");
        }

        public string GetDownloadDestinationFolder()
        {
            return Get("DownloadDestinationFolder");
        }

        public string GetCentralMessageQueueName()
        {
            return Get("CentralMessageQueueName");
        }

        public string GetCentralMessageQueueMachine()
        {
            return Get("CentralMessageQueueMachine");
        }

        private string Get(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return string.Empty;
            }

            return ConfigurationManager.AppSettings[key] ?? string.Empty;
        }
    }
}
