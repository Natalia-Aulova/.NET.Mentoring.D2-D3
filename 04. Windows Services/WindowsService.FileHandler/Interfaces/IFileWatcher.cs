namespace WindowsService.FileHandler.Interfaces
{
    public interface IFileWatcher
    {
        void Start(string sourceFolderPath, string destinationFolderPath, int saveTimeout);

        void Stop();
    }
}
