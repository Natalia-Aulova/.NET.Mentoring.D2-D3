namespace MSMQ.StreamScanning.Common.Models
{
    public class UpdatePageTimeoutMessage : ICentralCommand
    {
        public int Timeout { get; set; }
    }
}
