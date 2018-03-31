using System.Configuration;

namespace IQueryableExample.Services
{
    public class SettingsProvider : ISettingsProvider
    {
        public string GetSetting(string settingName)
        {
            return ConfigurationManager.AppSettings[settingName];
        }
    }
}
