using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using MQTTnet.AspNetCore;

namespace AspNetCoreMqttServerTest
{
    public class Program
    {
        //https://github.com/chkr1011/MQTTnet/wiki/Server
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureKestrel((context, options) =>
                    {
                        options.ListenAnyIP(1883, l => l.UseMqtt());
                    });
                    webBuilder.UseStartup<Startup>();
                })
                .ConfigureServices(services =>
                {
                    services.AddHostedMqttServer(builder =>
                    {
                        
                        builder.WithDefaultEndpointPort(1883);
                        builder.WithApplicationMessageInterceptor(context =>
                        {
                            var payload = Encoding.UTF8.GetString(context.ApplicationMessage.Payload);
                        });
                    });
                    services.AddMqttConnectionHandler();
                    services.AddMqttWebSocketServerAdapter();
                });
    }
}
