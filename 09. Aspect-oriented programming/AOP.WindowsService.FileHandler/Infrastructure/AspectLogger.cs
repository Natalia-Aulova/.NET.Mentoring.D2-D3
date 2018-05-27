using System.Collections.Generic;
using AOP.WindowsService.FileHandler.Infrastructure.Interfaces;
using Newtonsoft.Json;
using NLog;

namespace AOP.WindowsService.FileHandler.Infrastructure
{
    public class AspectLogger : IAspectLogger
    {
        private const string NotSerializable = "Not serializable";
        private readonly ILogger _logger;

        public AspectLogger(ILogger logger)
        {
            _logger = logger;
        }

        public void LogMethodEntry(string className, string methodName, Dictionary<string, object> parameters)
        {
            var serializedArgs = GetSerializedObject(parameters);
            _logger.Trace($"Start method: {className}.{methodName}; Args: {serializedArgs}");
        }

        public void LogMethodExit(string className, string methodName, object returnValue)
        {
            var serializedReturnValue = GetSerializedObject(returnValue);
            _logger.Trace($"End method: {className}.{methodName}; Return value: {serializedReturnValue}");
        }

        private string GetSerializedObject(object value)
        {
            try
            {
               return JsonConvert.SerializeObject(value);
            }
            catch
            {
                return NotSerializable;
            }
        }
    }
}
