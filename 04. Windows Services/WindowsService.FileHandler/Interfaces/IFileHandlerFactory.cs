namespace WindowsService.FileHandler.Interfaces
{
    public interface IFileHandlerFactory
    {
        IFileHandler GetHandler(int loggerNumber);
    }
}
