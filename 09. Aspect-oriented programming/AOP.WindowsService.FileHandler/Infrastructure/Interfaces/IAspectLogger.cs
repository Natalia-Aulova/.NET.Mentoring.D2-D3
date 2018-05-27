using System.Collections.Generic;

namespace AOP.WindowsService.FileHandler.Infrastructure.Interfaces
{
    public interface IAspectLogger
    {
        void LogMethodEntry(string className, string methodName, Dictionary<string, object> parameters);

        void LogMethodExit(string className, string methodName, object returnValue);
    }
}
