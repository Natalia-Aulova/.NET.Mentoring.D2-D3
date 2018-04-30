using System;
using MSMQ.StreamScanning.Models;

namespace MSMQ.StreamScanning.Interfaces
{
    public interface IFileHandler
    {
        event EventHandler<DocumentEventArgs> DocumentSaved;

        ServiceActivity CurrentActivity { get; }

        void Start(string sourceFolderPath, int saveTimeout);

        void Stop();

        void ChangePageTimeout(int newTimeout);
    }
}
