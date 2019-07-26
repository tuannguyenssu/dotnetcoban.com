using Polly;
using System;

namespace PollyTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var policy = Policy.Handle<Exception>().Retry(0);
            policy.Execute(() =>
            {

            });

            Console.ReadKey();
        }
    }
}
