namespace MSMQ.StreamScanning.Common.Models
{
    public class ServiceInfoMessage
    {
        public string MachineName { get; set; }

        public string CurrentServiceActivity { get; set; }

        public int PageTimeout { get; set; }
    }
}
