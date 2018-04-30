using System.Text.RegularExpressions;
using MSMQ.StreamScanning.CentralService.Interfaces;
using MSMQ.StreamScanning.Common.Models;

namespace MSMQ.StreamScanning.CentralService.Services
{
    public class CommandParser : ICommandParser
    {
        private readonly string _updateStatusTemplate = "^update-status$";
        private readonly string _changePageTimeoutTemplate = @"^change-page-timeout:(\d+)$";

        public ICentralCommand Parse(string command)
        {
            ICentralCommand result;

            if (TryParseUpdateStatusCommand(command, out result) ||
                TryParseChangePageTimeoutCommand(command, out result))
            {
                return result;
            }
            
            return null;
        }

        private bool TryParseUpdateStatusCommand(string input, out ICentralCommand command)
        {
            var regex = new Regex(_updateStatusTemplate);

            if (regex.IsMatch(input))
            {
                command = new UpdateStatusMessage();
                return true;
            }

            command = null;
            return false;
        }

        private bool TryParseChangePageTimeoutCommand(string input, out ICentralCommand command)
        {
            var regex = new Regex(_changePageTimeoutTemplate);

            if (regex.IsMatch(input))
            {
                command = new UpdatePageTimeoutMessage
                {
                    Timeout = int.Parse(regex.Match(input).Groups[1].Value)
                };

                return true;
            }

            command = null;
            return false;
        }
    }
}
