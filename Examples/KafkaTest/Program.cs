using System;
using System.Threading;
using Confluent.Kafka;

namespace KafkaTest
{
    class Program
    {
        private static void TestProducer()
        {
            var conf = new ProducerConfig { BootstrapServers = "localhost:9092" };

            Action<DeliveryReport<Null, string>> handler = r => 
                Console.WriteLine(!r.Error.IsError
                    ? $"Delivered message to {r.TopicPartitionOffset}"
                    : $"Delivery Error: {r.Error.Reason}");

            using (var p = new ProducerBuilder<Null, string>(conf).Build())
            {
                for (int i=0; i<2; ++i)
                {
                    p.Produce("my-topic", new Message<Null, string> { Value = i.ToString() }, handler);
                }

                // wait for up to 10 seconds for any inflight messages to be delivered.
                p.Flush(TimeSpan.FromSeconds(10));
            }            
        }

        private static void TestConsumer()
        {
            var conf = new ConsumerConfig
            { 
                GroupId = "test-consumer-group",
                BootstrapServers = "localhost:9092",
                // Note: The AutoOffsetReset property determines the start offset in the event
                // there are not yet any committed offsets for the consumer group for the
                // topic/partitions of interest. By default, offsets are committed
                // automatically, so in this example, consumption will only start from the
                // earliest message in the topic 'my-topic' the first time you run the program.
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            using (var c = new ConsumerBuilder<Ignore, string>(conf).Build())
            {
                c.Subscribe("my-topic");

                CancellationTokenSource cts = new CancellationTokenSource();
                Console.CancelKeyPress += (_, e) => {
                    e.Cancel = true; // prevent the process from terminating.
                    cts.Cancel();
                };

                try
                {
                    while (true)
                    {
                        try
                        {
                            var cr = c.Consume(cts.Token);
                            Console.WriteLine($"Consumed message '{cr.Value}' at: '{cr.TopicPartitionOffset}'.");
                        }
                        catch (ConsumeException e)
                        {
                            Console.WriteLine($"Error occured: {e.Error.Reason}");
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    // Ensure the consumer leaves the group cleanly and final offsets are committed.
                    c.Close();
                }
            }            
        }
        static void Main(string[] args)
        {
            TestProducer();
            TestConsumer();
        }
    }
}
