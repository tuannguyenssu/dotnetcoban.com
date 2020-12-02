using System;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using KafkaTest.Shared;

namespace KafkaTest.Subscriber
{
    class Program
    {
        static void Main(string[] args)
        {
            var kafkaOptions = new KafkaOptions()
            {
                BootstrapServers = "moped-01.srvs.cloudkafka.com:9094,moped-02.srvs.cloudkafka.com:9094,moped-03.srvs.cloudkafka.com:9094",
                UserName = "g77rbl7m",
                Password = "Uqr_uguu9U2Z-W25zP-_IzKw9eX_wmM1"
            };

            var config = new ConsumerConfig()
            {
                BootstrapServers = kafkaOptions.BootstrapServers,
                SecurityProtocol = SecurityProtocol.SaslSsl,
                SaslMechanism = SaslMechanism.ScramSha256,
                SaslUsername = kafkaOptions.UserName,
                SaslPassword = kafkaOptions.Password,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                GroupId = "cloudkarafka-test"
            };
            var receiver = new ConsumerBuilder<Ignore, string>(config).Build();
            receiver.Subscribe(Constants.TopicName);
            var cts = new CancellationTokenSource();
            var cancellationToken = cts.Token;

            Task.Factory.StartNew(() =>
            {
                try
                {
                    StartReceiving(receiver, cancellationToken);
                }
                catch (OperationCanceledException)
                {
                    Console.WriteLine("Closing consumer.");
                    receiver.Close();
                }
            }, cancellationToken);

            Console.ReadKey();
        }

        private static void StartReceiving(IConsumer<Ignore,string> consumer, CancellationToken cancellationToken)
        {
            while (true)
            {
                try
                {
                    var consumeResult = consumer.Consume(cancellationToken);

                    if (consumeResult.IsPartitionEOF)
                    {
                        continue;
                    }

                    //var data = JsonSerializer.Deserialize<VehicleData>(consumeResult.Message.Value);
                    //Console.WriteLine(JsonSerializer.Serialize(data));
                    Console.WriteLine($"{DateTime.Now} : {consumeResult.Message.Value}");
                }
                catch (ConsumeException e)
                {
                    Console.WriteLine($"Consume error: {e.Error.Reason}");
                }
            }
        }

    }
}
