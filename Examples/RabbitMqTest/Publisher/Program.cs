using System;
using System.Text.Json;
using System.Timers;
using RabbitMQ.Client;
using Shared;

namespace Publisher
{
    class Program
    {
        private static IModel _channel;
        static void Main(string[] args)
        {
            var rabbitMqOptions = new RabbitMqOptions()
            {
                ConnectionString = "amqps://iujycclw:T4BfoAt3Rfk-tXkcGLignzbGHAe1LU_v@cougar.rmq.cloudamqp.com/iujycclw"
            };
            var connectionFactory = new ConnectionFactory()
            {
                Uri = new Uri(rabbitMqOptions.ConnectionString)
            };
            var connection = connectionFactory.CreateConnection();

            _channel = connection.CreateModel();
            //SetupDirectExchange();
            SetupTopicExchange();

            var timer = new Timer(200)
            {
                AutoReset = true,
            };
            timer.Elapsed += TimerOnElapsed;
            timer.Start();

            Console.ReadKey();

        }

        private static void SetupDirectExchange()
        {
            _channel.ExchangeDeclare(Shared.Constants.DirectExchangeName, ExchangeType.Direct, true, true);
        }

        private static void SetupTopicExchange()
        {
            _channel.ExchangeDeclare(Shared.Constants.TopicExchangeName, ExchangeType.Topic, true, true);
        }

        private static void TimerOnElapsed(object sender, ElapsedEventArgs e)
        {
            var vehicleData = new VehicleData()
            {
                CustomerId = 1000,
                VehicleName = "29A2929"
            };

            var bytes = JsonSerializer.SerializeToUtf8Bytes(vehicleData);
            //_channel.BasicPublish(Shared.Constants.DirectExchangeName, Shared.Constants.DirectExchangeRoutingKey, null, bytes);
            _channel.BasicPublish(Shared.Constants.TopicExchangeName, Shared.Constants.TopicExchangeRoutingKey, null, bytes);
            Console.WriteLine("Data: " + JsonSerializer.Serialize(vehicleData));
        }
    }
}
