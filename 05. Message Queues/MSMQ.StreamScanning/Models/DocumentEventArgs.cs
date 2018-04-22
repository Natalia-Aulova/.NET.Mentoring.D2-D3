namespace MSMQ.StreamScanning.Models
{
    public class DocumentEventArgs
    {
        public string FilePath { get; }

        public DocumentEventArgs(string filePath)
        {
            FilePath = filePath;
        }
    }
}
