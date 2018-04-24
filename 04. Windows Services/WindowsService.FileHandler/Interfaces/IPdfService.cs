using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace WindowsService.FileHandler.Interfaces
{
    public interface IPdfService
    {
        Task SaveDocument(IEnumerable<string> filePaths, string destinationFileName, CancellationToken token);
    }
}
