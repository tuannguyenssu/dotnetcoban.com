using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MQTTClientTest
{
    class Program
    {
        private const string MyTopic = "myTopic";
        private const string MQTTServerUrl = "localhost";
        private const int MQTTPort = 18833;
        static async Task Main(string[] args)
        {
            MqttFactory factory = new MqttFactory();
            // Create a new MQTT client.            
            var mqttClient = factory.CreateMqttClient();

            var options = new MqttClientOptionsBuilder()
            .WithTcpServer(MQTTServerUrl, MQTTPort)
            .Build();           

            // Reconnecting event handler
            mqttClient.UseDisconnectedHandler(async e =>
            {
                Console.WriteLine("### DISCONNECTED FROM SERVER ###");
                await Task.Delay(TimeSpan.FromSeconds(5));
                try
                {
                    await mqttClient.ConnectAsync(options, CancellationToken.None);
                }
                catch
                {
                    Console.WriteLine("### RECONNECTING FAILED ###");
                }
            });       

            // Message received event handler
            // Consuming messages
            mqttClient.UseApplicationMessageReceivedHandler(e =>
            {
                Console.WriteLine("### RECEIVED APPLICATION MESSAGE ###");
                Console.WriteLine($"+ Topic = {e.ApplicationMessage.Topic}");
                Console.WriteLine($"+ Payload = {Encoding.UTF8.GetString(e.ApplicationMessage.Payload)}");
                Console.WriteLine($"+ QoS = {e.ApplicationMessage.QualityOfServiceLevel}");
                Console.WriteLine($"+ Retain = {e.ApplicationMessage.Retain}");
                Console.WriteLine();
            });     

            // Connected event handler
            mqttClient.UseConnectedHandler(async e =>
            {
                Console.WriteLine("### CONNECTED WITH SERVER ###");

                // Subscribe to a topic
                await mqttClient.SubscribeAsync(new TopicFilterBuilder().WithTopic(MyTopic).Build());

                // Subscribe all topics
                // await mqttClient.SubscribeAsync("#");

                Console.WriteLine("### SUBSCRIBED ###");
            });                         

            // Try to connect to MQTT server
            await mqttClient.ConnectAsync(options, CancellationToken.None);    

            // Publishing messages  
            var message = new MqttApplicationMessageBuilder()
                .WithTopic(MyTopic)
                .WithPayload("Hello mqtt")
                .Build();                 
            await mqttClient.PublishAsync(message);
            Console.WriteLine($"### SENT MESSAGE {Encoding.UTF8.GetString(message.Payload)} TO SERVER ");
            Console.ReadLine();
        }
    }
}
