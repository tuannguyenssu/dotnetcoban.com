using System;
using System.Text.Json;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Shared;

namespace Subscriber
{
    class Program
    {
        static void Main(string[] args)
        {
            var rabbitMqOptions = new RabbitMqOptions()
            {
                ConnectionString = "amqps://iujycclw:T4BfoAt3Rfk-tXkcGLignzbGHAe1LU_v@cougar.rmq.cloudamqp.com/iujycclw"
            };
            var connectionFactory = new ConnectionFactory()
            {
                Uri = new Uri(rabbitMqOptions.ConnectionString),
                DispatchConsumersAsync = true
            };
            var connection = connectionFactory.CreateConnection();
            //TestDirectExchange(connection);
            TestTopicExchange(connection);

            Console.ReadKey();
        }

        private static void TestTopicExchange(IConnection connection)
        {
            var channel = connection.CreateModel();
            channel.ExchangeDeclare(Shared.Constants.TopicExchangeName, ExchangeType.Topic, true, true);
            channel.QueueDeclare(Shared.Constants.TopicExchangeQueue, false, false, false);
            //var routingKey = "#";
            //var routingKey = "1010.#";
            var routingKey = Shared.Constants.TopicExchangeRoutingKey;
            //channel.QueueUnbind(Shared.Constants.TopicExchangeQueue, Shared.Constants.TopicExchangeName, routingKey);
            channel.QueueBind(Shared.Constants.TopicExchangeQueue, Shared.Constants.TopicExchangeName, routingKey);
            var subscriber = new AsyncEventingBasicConsumer(channel);

            subscriber.Received += DataReceived;

            channel.BasicConsume(Shared.Constants.TopicExchangeQueue, true, subscriber);
        }

        private static void TestDirectExchange(IConnection connection)
        {
            var channel = connection.CreateModel();
            channel.ExchangeDeclare(Shared.Constants.DirectExchangeName, ExchangeType.Direct, true, true);
            channel.QueueDeclare(Shared.Constants.DirectExchangeQueue, false, false, false);
            channel.QueueBind(Shared.Constants.DirectExchangeQueue, Shared.Constants.DirectExchangeName, Shared.Constants.DirectExchangeRoutingKey);
            var subscriber = new AsyncEventingBasicConsumer(channel);

            subscriber.Received += DataReceived;

            channel.BasicConsume(Shared.Constants.DirectExchangeQueue, true, subscriber);
        }

        private static Task DataReceived(object sender, BasicDeliverEventArgs args)
        {
            //Console.WriteLine("Exchange: " + args.Exchange);
            //Console.WriteLine("RoutingKey: " + args.RoutingKey);

            //Console.WriteLine("Delivery: " + args.DeliveryTag);

            var data = JsonSerializer.Deserialize<VehicleData>(args.Body.Span);
            Console.WriteLine($"{DateTime.Now} {JsonSerializer.Serialize(data)}");
            return Task.CompletedTask;
        }
    }
}
