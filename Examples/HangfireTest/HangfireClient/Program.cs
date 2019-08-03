using Hangfire;
using System;

namespace HangfireClient
{
    class Program
    {
        static void Main(string[] args)
        {
            string dbConnection = "Server=localhost;Database=HangfireTest;User=sa;Password=Pass@word;";
            GlobalConfiguration.Configuration.UseSqlServerStorage(dbConnection);
            BackgroundJob.Enqueue(() => Console.WriteLine("Hello!"));
        }
    }
}
