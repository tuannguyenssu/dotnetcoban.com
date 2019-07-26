using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;
using System.Timers;

namespace SignalRClient
{
    class Program
    {
        static HubConnection connection;
        static void Main(string[] args)
        {
            var hubUrl = "http://localhost:52870/ChatHub";

            connection = new HubConnectionBuilder()
                .WithUrl(hubUrl)
                .Build();

            connection.Closed += async (error) =>
            {
                await Task.Delay(new Random().Next(0, 5) * 1000);
                await connection.StartAsync();
            };

            connection.On<string, string>("ReceiveMessage", (user, message) =>
            {
                var newMessage = $"{user}: {message}";
                Console.WriteLine(newMessage);
            });

            Timer timer = new Timer();
            timer.Elapsed += Timer_Elapsed;
            timer.Interval = 10;
            try
            {
                connection.StartAsync().Wait();
                timer.Start();
                Console.WriteLine("Connection started");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.WriteLine("Press \'q\' to quit the sample.");
            while (Console.Read() != 'q') ;
        }

        private static void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                connection.InvokeAsync("SendMessage",
                    Guid.NewGuid().ToString(), DateTime.Now.ToString()).Wait();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
