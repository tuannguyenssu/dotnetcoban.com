using System.Threading;
using System.Threading.Tasks;
using FubarDev.FtpServer;
using Microsoft.Extensions.Hosting;

namespace FtpServerNetCore
{
    public class HostedFtpService : IHostedService
    {
        private readonly IFtpServerHost _ftpServerHost;

        public HostedFtpService(
            IFtpServerHost ftpServerHost)
        {
            _ftpServerHost = ftpServerHost;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return _ftpServerHost.StartAsync(cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return _ftpServerHost.StopAsync(cancellationToken);
        }
    }
}
