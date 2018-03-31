using System.Configuration;

namespace IQueryableExample.ConsoleApp.Services
{
    public class SettingsProvider : ISettingsProvider
    {
        public string GetSetting(string settingName)
        {
            return ConfigurationManager.AppSettings[settingName];
        }
    }
}
