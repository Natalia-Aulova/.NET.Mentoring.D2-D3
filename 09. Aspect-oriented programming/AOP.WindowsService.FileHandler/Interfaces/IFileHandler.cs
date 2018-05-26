namespace AOP.WindowsService.FileHandler.Interfaces
{
    public interface IFileHandler
    {
        void Start(string sourceFolderPath, int saveTimeout);

        void Stop();
    }
}
