using AsyncAwait.WPF.Entities;

namespace AsyncAwait.WPF.Services
{
    public static class NameHelper
    {
        public static string GetCancelButtonName(int id)
        {
            return ControlTemplates.CancelButtonTemplate + id;
        }

        public static string GetStatusTextBlockName(int id)
        {
            return ControlTemplates.StatusTextBlockTemplate + id;
        }

        public static int GetIdFromCancelButtonName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return -1;
            }

            int result;
            
            return int.TryParse(name.Substring(ControlTemplates.CancelButtonTemplate.Length), out result)
                ? result
                : -1;
        }
    }
}
