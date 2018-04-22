using System.Configuration;
using MSMQ.StreamScanning.Common.Interfaces;

namespace MSMQ.StreamScanning.Common.Providers
{
    public class SettingsProvider : ISettingsProvider
    {
        public string GetSetting(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return string.Empty;
            }

            return ConfigurationManager.AppSettings[key] ?? string.Empty;
        }
    }
}
