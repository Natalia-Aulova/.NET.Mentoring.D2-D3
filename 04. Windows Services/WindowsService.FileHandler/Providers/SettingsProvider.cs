using System.Configuration;
using WindowsService.FileHandler.Interfaces;

namespace WindowsService.FileHandler.Providers
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
