using System;

namespace MSMQ.StreamScanning.Models
{
    public class DocumentEventArgs : EventArgs
    {
        public string FilePath { get; }

        public DocumentEventArgs(string filePath)
        {
            FilePath = filePath;
        }
    }
}
