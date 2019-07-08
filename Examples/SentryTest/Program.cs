using System;
using Sentry;

namespace SentryTest
{
    class Program
    {
        static void Main(string[] args)
        {
            string projectUrl = "https://0bdae487b66648469787f7bb2f297ba5@sentry.io/1495681";
            using (SentrySdk.Init(projectUrl))
            {
                int a = 1;
                int b = 0;
                Console.WriteLine(a / b);
            }            
        }
    }
}
