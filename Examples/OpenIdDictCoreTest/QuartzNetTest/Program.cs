using System;
using System.Threading.Tasks;

namespace QuartzNetTest
{
    //https://github.com/quartznet/quartznet
    class Program
    {
        static async Task Main(string[] args)
        {
            await JobScheduler.Start();

            Console.ReadKey();
        }
    }
}
