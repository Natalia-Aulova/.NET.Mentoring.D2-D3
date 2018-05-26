namespace AOP.WindowsService.FileHandler.Interfaces
{
    public interface ISettingsProvider
    {
        string GetBrokenFolderName();

        string GetDestinationFolderPath();

        string GetNameTemplate();

        int GetPageTimeout();

        int GetRetryCount();

        int GetRetryDelay();

        string[] GetSourceFolderPaths();

        string[] GetSupportedExtensions();
    }
}
