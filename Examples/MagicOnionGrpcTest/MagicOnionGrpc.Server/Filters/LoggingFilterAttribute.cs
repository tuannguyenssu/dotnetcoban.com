using System;
using System.Threading.Tasks;
using MagicOnion.Server;
using Microsoft.Extensions.Logging;

namespace MagicOnionGrpc.Server.Filters
{
    public class LoggingFilterAttribute : MagicOnionFilterAttribute
    {
        private readonly ILogger<LoggingFilterAttribute> _logger;

        public LoggingFilterAttribute(ILogger<LoggingFilterAttribute> logger)
        {
            _logger = logger;
        }

        public override async ValueTask Invoke(ServiceContext context, Func<ServiceContext, ValueTask> next)
        {
            try
            {
                _logger.LogInformation($"Begin {context.MethodInfo.Name}");
                await next(context);
                _logger.LogInformation($"End {context.MethodInfo.Name}");
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Invoke Failed");
            }
            finally
            {
                /* on finally */
            }
        }
    }
}
