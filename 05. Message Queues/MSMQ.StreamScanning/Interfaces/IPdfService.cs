using System.Collections.Generic;

namespace MSMQ.StreamScanning.Interfaces
{
    public interface IPdfService
    {
        string SaveDocument(IEnumerable<string> filePaths, string destinationFileName);
    }
}
