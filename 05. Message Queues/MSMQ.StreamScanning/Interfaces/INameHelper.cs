namespace MSMQ.StreamScanning.Interfaces
{
    public interface INameHelper
    {
        bool IsNameMatch(string fileName);

        int GetFileNameNumber(string fileName);
    }
}
