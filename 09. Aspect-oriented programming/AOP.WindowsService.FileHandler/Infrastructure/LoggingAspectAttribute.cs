using System;
using System.Collections.Generic;
using System.Linq;
using AOP.WindowsService.FileHandler.Infrastructure.Interfaces;
using NLog;
using PostSharp.Aspects;

namespace AOP.WindowsService.FileHandler.Infrastructure
{
	[Serializable]
    public class LoggingAspectAttribute : OnMethodBoundaryAspect
    {
        private IAspectLogger _aspectLogger;

        private IAspectLogger AspectLogger
        {
            get
            {
                if (_aspectLogger == null)
                {
                    InitializeLogger();
                }

                return _aspectLogger;
            }
        }
        
        public override void OnEntry(MethodExecutionArgs args)
        {
            AspectLogger.LogMethodEntry(
                args.Method.DeclaringType.Name, 
                args.Method.Name, 
                GetParameters(args));
        }

        public override void OnSuccess(MethodExecutionArgs args)
        {
            AspectLogger.LogMethodExit(
                args.Method.DeclaringType.Name,
                args.Method.Name,
                args.ReturnValue);
        }

        private void InitializeLogger()
        {
            _aspectLogger = new AspectLogger(LogManager.GetLogger(nameof(AspectLogger)));
        }

        private Dictionary<string, object> GetParameters(MethodExecutionArgs args)
        {
            var parameters = args.Method.GetParameters()
                .Select((x, i) => new { ParamName = x.Name, ParamValue = args.Arguments[i] })
                .ToDictionary(x => x.ParamName, x => x.ParamValue);

            return parameters ?? new Dictionary<string, object>();
        }
    }
}
