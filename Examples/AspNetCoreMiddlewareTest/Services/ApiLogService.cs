using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreMiddlewareTest.Models;
using Microsoft.Extensions.Logging;

namespace AspNetCoreMiddlewareTest.Services
{
    public interface IApiLogService
    {
        Task Log(ApiLogItem apiLogItem);
    }
    public class ApiLogService : IApiLogService
    {
        private ILogger<ApiLogService> _logger;

        public ApiLogService(ILogger<ApiLogService> logger)
        {
            _logger = logger;
        }

        public async Task Log(ApiLogItem apiLogItem)
        {
            _logger.LogInformation(apiLogItem.ToString());
            await Task.CompletedTask;
        }
    }
}
