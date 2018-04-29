namespace MSMQ.StreamScanning.Common.Interfaces
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

        string GetMessageQueueName();

        string GetDownloadDestinationFolder();

        string GetCentralMessageQueueName();

        string GetCentralMessageQueueMachine();

        int GetReportingTimeout();

        string GetInputServiceStatusesFile();
    }
}
