namespace WindowsService.FileHandler.Interfaces
{
    public interface IFileHandler
    {
        void Start(string sourceFolderPath, string destinationFolderPath, int saveTimeout);

        void Stop();
    }
}
