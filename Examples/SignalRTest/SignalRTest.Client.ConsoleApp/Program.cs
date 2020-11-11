using System;
using System.Threading.Tasks;
using SignalRTest.Shared;

namespace SignalRTest.Client.ConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Starting App...");
            try
            {
                string username;
                do
                {
                    Console.WriteLine("Enter your name: ");
                    username = Console.ReadLine();
                }
                while (string.IsNullOrWhiteSpace(username));

                const string url = "http://localhost:8000";
                var client = new ChatClient(username, url);

                client.MessageReceived += MessageReceived;

                await client.StartAsync();
                bool exit = false;
                Console.WriteLine("Enter message, or 'exit' to quit");
                do
                {
                    var message = Console.ReadLine();
                    await client.SendAsync(message);

                    if (message?.ToLower() == "exit")
                        exit = true;

                } while (!exit);

                Console.WriteLine("press any key to exit");
                Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR: " + e.Message);
            }
        }

        private static void MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            Console.WriteLine($"[{e.Username}] {e.Message}");
        }
    }
}
