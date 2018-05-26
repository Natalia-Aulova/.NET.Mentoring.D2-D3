using System.Collections.Generic;

namespace AOP.WindowsService.FileHandler.Interfaces
{
    public interface IPdfService
    {
        void SaveDocument(IEnumerable<string> filePaths, string destinationFileName);
    }
}
