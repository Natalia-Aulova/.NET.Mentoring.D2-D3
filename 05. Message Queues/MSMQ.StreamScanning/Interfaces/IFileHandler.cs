using System;
using MSMQ.StreamScanning.Models;

namespace MSMQ.StreamScanning.Interfaces
{
    public interface IFileHandler
    {
        event EventHandler<DocumentEventArgs> DocumentSaved;

        void Start(string sourceFolderPath, string destinationFolderPath, int saveTimeout);

        void Stop();
    }
}
