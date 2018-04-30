using MSMQ.StreamScanning.Common.Models;

namespace MSMQ.StreamScanning.CentralService.Interfaces
{
    public interface ICommandParser
    {
        ICentralCommand Parse(string command);
    }
}
