using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using NATS.Client;
using NATS.Client.Rx;
using NATS.Client.Rx.Ops;

namespace NatsTest
{
    //https://github.com/nats-io/nats.net
    //https://docs.nats.io/developing-with-nats/connecting

    class Program
    {
        private static IConnection _connection;
        static void Main(string[] args)
        {
            //TestPubSub();
            TestRequestReply();
        }

        private static void TestRequestReply()
        {
            string[] servers = {
                "nats://demo.nats.io:4222"
            };
            var opts = ConnectionFactory.GetDefaultOptions();
            opts.MaxReconnect = 2;
            opts.ReconnectWait = 1000;
            opts.NoRandomize = true;
            opts.Servers = servers;
            _connection = new ConnectionFactory().CreateConnection(opts);
            var receiver = _connection.SubscribeAsync("temperatures");
            receiver.MessageHandler += ReceiverOnMessageHandler;
            receiver.Start();
            var cts = new CancellationTokenSource();

            Task.Run(async () =>
            {
                var rnd = new Random();

                while (!cts.IsCancellationRequested)
                {
                    var response = _connection.Request("temperatures", Encoding.UTF8.GetBytes("request"));
                    Console.WriteLine($"{response.Subject} {Encoding.UTF8.GetString(response.Data)}");
                    await Task.Delay(1000, cts.Token);
                }
            }, cts.Token);

            Console.ReadKey();
            cts.Cancel();
            _connection.Drain();
            _connection.Close();
        }

        private static void ReceiverOnMessageHandler(object sender, MsgHandlerEventArgs e)
        {
            Console.WriteLine($"{e.Message.Subject} {Encoding.UTF8.GetString(e.Message.Data)}");
            var reply = new Msg { Subject = e.Message.Reply, Data = Encoding.UTF8.GetBytes("reply") };
            _connection.Publish(reply);
        }

        private static void TestPubSub()
        {
            string[] servers = {
                "nats://demo.nats.io:4222"
            };
            var opts = ConnectionFactory.GetDefaultOptions();
            opts.MaxReconnect = 2;
            opts.ReconnectWait = 1000;
            opts.NoRandomize = true;
            opts.Servers = servers;
            using var connection = new ConnectionFactory().CreateEncodedConnection(opts);
            connection.OnSerialize = OnSerialize;
            connection.OnDeserialize = OnDeserialize;
            var receiver = connection.SubscribeAsync("temperatures", EncodedReceiverOnMessageHandler);
            receiver.Start();
            var cts = new CancellationTokenSource();

            Task.Run(async () =>
            {
                var rnd = new Random();

                while (!cts.IsCancellationRequested)
                {
                    var temperature = new Temperature()
                    {
                        Celsius = rnd.Next(-10, 40)
                    };
                    connection.Publish("temperatures", temperature);

                    await Task.Delay(1000, cts.Token);
                }
            }, cts.Token);

            Console.ReadKey();
            cts.Cancel();
            connection.Drain();
            connection.Close();
        }

        private static byte[] OnSerialize(object obj)
        {
            return Utf8Json.JsonSerializer.Serialize(obj);

            //System.Text.Json
            //return JsonSerializer.SerializeToUtf8Bytes(obj);
        }

        private static object OnDeserialize(byte[] data)
        {
            return Utf8Json.JsonSerializer.Deserialize<Temperature>(data);

            //System.Text.Json
            //return JsonSerializer.Deserialize<Temperature>(data);
        }

        private static void EncodedReceiverOnMessageHandler(object sender, EncodedMessageEventArgs e)
        {
            Temperature temperature = (Temperature)e.ReceivedObject;
            Console.WriteLine($"{temperature.Celsius}C");

            var reply = new Msg {Subject = e.Message.Reply, Data = Encoding.UTF8.GetBytes("reply")};
            _connection.Publish(reply);
        }

        private static void TestRx()
        {
            string[] servers = {
                "nats://demo.nats.io:4222"
            };
            var opts = ConnectionFactory.GetDefaultOptions();
            opts.MaxReconnect = 2;
            opts.ReconnectWait = 1000;
            opts.NoRandomize = true;
            opts.Servers = servers;
            using var cn = new ConnectionFactory().CreateConnection(opts);
            var temperatures =
                cn.Observe("temperatures")
                    .Where(m => m.Data?.Any() == true)
                    .Select(m => BitConverter.ToInt32(m.Data, 0));

            temperatures.Subscribe(t => Console.WriteLine($"{t}C"));

            temperatures.Subscribe(t => Console.WriteLine($"{(t * 9 / 5) + 32}F"));

            var cts = new CancellationTokenSource();

            Task.Run(async () =>
            {
                var rnd = new Random();

                while (!cts.IsCancellationRequested)
                {
                    cn.Publish("temperatures", BitConverter.GetBytes(rnd.Next(-10, 40)));

                    await Task.Delay(1000, cts.Token);
                }
            }, cts.Token);

            Console.ReadKey();
            cts.Cancel();
            cn.Drain();
            cn.Close();
        }
    }
}
