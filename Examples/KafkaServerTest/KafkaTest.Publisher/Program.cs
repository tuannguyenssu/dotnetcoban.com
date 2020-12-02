using System;
using System.Text.Json;
using System.Timers;
using Confluent.Kafka;
using KafkaTest.Shared;

namespace KafkaTest.Publisher
{
    class Program
    {
        private static IProducer<Null, string> _producer;
        static void Main(string[] args)
        {
            var kafkaOptions = new KafkaOptions()
            {
                BootstrapServers = "moped-01.srvs.cloudkafka.com:9094,moped-02.srvs.cloudkafka.com:9094,moped-03.srvs.cloudkafka.com:9094",
                UserName = "g77rbl7m",
                Password = "Uqr_uguu9U2Z-W25zP-_IzKw9eX_wmM1"
            };

            var config = new ProducerConfig()
            {
                BootstrapServers = kafkaOptions.BootstrapServers,
                SecurityProtocol = SecurityProtocol.SaslSsl,
                SaslMechanism = SaslMechanism.ScramSha256,
                SaslUsername = kafkaOptions.UserName,
                SaslPassword = kafkaOptions.Password
            };

            _producer = new ProducerBuilder<Null, string>(config).Build();

            var timer = new Timer(200)
            {
                AutoReset = true,
            };
            timer.Elapsed += TimerOnElapsed;
            timer.Start();

            Console.ReadKey();
        }

        private static async void TimerOnElapsed(object sender, ElapsedEventArgs e)
        {
            var vehicleData = new VehicleData()
            {
                CustomerId = 1000,
                VehicleName = "29A2929"
            };
            var r = await _producer.ProduceAsync(Constants.TopicName, new Message<Null, string>() { Value = JsonSerializer.Serialize(vehicleData) });
            //Console.WriteLine(r.TopicPartitionOffset);
            Console.WriteLine($"{DateTime.Now} {JsonSerializer.Serialize(vehicleData)}");
        }
    }
}
