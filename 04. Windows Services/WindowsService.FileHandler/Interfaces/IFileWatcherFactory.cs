namespace WindowsService.FileHandler.Interfaces
{
    public interface IFileWatcherFactory
    {
        IFileWatcher GetWatcher();
    }
}
