namespace IQueryableExample.ConsoleApp.Services
{
    public interface ISettingsProvider
    {
        string GetSetting(string settingName);
    }
}
