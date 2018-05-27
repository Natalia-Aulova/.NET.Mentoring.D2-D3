using System.Collections.Generic;
using System.Linq;
using AOP.WindowsService.FileHandler.Infrastructure.Interfaces;
using Castle.DynamicProxy;

namespace AOP.WindowsService.FileHandler.Infrastructure
{
    public class LoggingInterceptor : IInterceptor
    {
        private readonly IAspectLogger _aspectLogger;

        public LoggingInterceptor(IAspectLogger aspectLogger)
        {
            _aspectLogger = aspectLogger;
        }

        public void Intercept(IInvocation invocation)
        {
            PreTrace(invocation);
            invocation.Proceed();
            PostTrace(invocation);
        }

        private void PreTrace(IInvocation invocation)
        {
            _aspectLogger.LogMethodEntry(
                invocation.TargetType.Name,
                invocation.MethodInvocationTarget.Name,
                GetParameters(invocation));
        }

        private void PostTrace(IInvocation invocation)
        {
            _aspectLogger.LogMethodExit(
                invocation.TargetType.Name,
                invocation.MethodInvocationTarget.Name,
                invocation.ReturnValue);
        }

        private Dictionary<string, object> GetParameters(IInvocation invocation)
        {
            var parameters = invocation.MethodInvocationTarget.GetParameters()
                .Select((x, i) => new { ParamName = x.Name, ParamValue = invocation.Arguments[i] })
                .ToDictionary(x => x.ParamName, x => x.ParamValue);

            return parameters ?? new Dictionary<string, object>();
        }
    }
}
