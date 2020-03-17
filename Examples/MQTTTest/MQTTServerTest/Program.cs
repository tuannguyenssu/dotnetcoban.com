using System;
using MQTTnet;
using MQTTnet.Protocol;
using MQTTnet.Server;

namespace MQTTServerTest
{
    class Program
    {
        //https://github.com/SeppPenner/SimpleMqttServer
        static void Main(string[] args)
        {
            var optionsBuilder = new MqttServerOptionsBuilder()
                .WithDefaultEndpoint()
                .WithDefaultEndpointPort(1883)
                .WithConnectionValidator(
                    c =>
                    {
                        //var currentUser = config.Users.FirstOrDefault(u => u.UserName == c.Username);

                        //if (currentUser == null)
                        //{
                        //    c.ReasonCode = MqttConnectReasonCode.BadUserNameOrPassword;
                        //    return;
                        //}

                        //if (c.Username != currentUser.UserName)
                        //{
                        //    c.ReasonCode = MqttConnectReasonCode.BadUserNameOrPassword;
                        //    return;
                        //}

                        //if (c.Password != currentUser.Password)
                        //{
                        //    c.ReasonCode = MqttConnectReasonCode.BadUserNameOrPassword;
                        //    return;
                        //}

                        c.ReasonCode = MqttConnectReasonCode.Success;
                    }).WithSubscriptionInterceptor(
                    c => { c.AcceptSubscription = true; }).WithApplicationMessageInterceptor(
                    c => { c.AcceptPublish = true; });

            var mqttServer = new MqttFactory().CreateMqttServer();
            mqttServer.StartAsync(optionsBuilder.Build());
        }
    }
}
