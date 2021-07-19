using System;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace ThreadingChannelTest
{
    class Program
    {
        private static readonly Channel<long> TestChannel = Channel.CreateBounded<long>(
            new BoundedChannelOptions(1)
            {
                FullMode = BoundedChannelFullMode.DropOldest
            });
        static void Main(string[] args)
        {
            _ = Task.Run(TestConsume);
            _ = Task.Run(TestProduce);
            Console.ReadKey();
        }

        private static async Task TestProduce()
        {
            while (true)
            {
                await TestChannel.Writer.WriteAsync(DateTime.Now.Ticks);
                await Task.Delay(10);
            }
        }

        private static async Task TestConsume()
        {
            while (true)
            {
                //var item = await TestChannel.Reader.ReadAsync();
                //Console.WriteLine(item);

                await foreach (var item in TestChannel.Reader.ReadAllAsync())
                {
                    Console.WriteLine(item);
                }
            }
        }
    }
}
