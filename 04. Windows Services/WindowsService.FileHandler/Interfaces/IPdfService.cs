using System.Collections.Generic;

namespace WindowsService.FileHandler.Interfaces
{
    public interface IPdfService
    {
        void SaveDocument(IEnumerable<string> filePaths, string destinationFileName);
    }
}
