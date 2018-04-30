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
            return GetInt("PageTimeout");
        }

        public int GetRetryCount()
        {
            return GetInt("RetryCount");
        }

        public int GetRetryDelay()
        {
            return GetInt("RetryDelay");
        }

        public string[] GetSourceFolderPaths()
        {
            return GetArray("SourceFolderPaths", "|");
        }

        public string[] GetSupportedExtensions()
        {
            return GetArray("SupportedExtensions", ",");
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

        public int GetReportingTimeout()
        {
            return GetInt("ReportingTimeout");
        }

        public string GetInputServiceStatusesFile()
        {
            return Get("InputServiceStatusesFile");
        }

        public string GetCommandFileName()
        {
            return Get("CommandFileName");
        }

        private int GetInt(string key)
        {
            return int.Parse(Get(key));
        }

        private string[] GetArray(string key, string separator)
        {
            return Get(key).Split(new[] { separator }, StringSplitOptions.RemoveEmptyEntries);
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
