using System;
using MagicOnionGrpc.Contract;

namespace MagicOnionGrpc.Client
{
    public class SampleStreamingHubReceiver : ISampleStreamingReceiver
    {
        public void OnJoin(string name)
        {
            Console.WriteLine($"{name} have joined");
        }

        public void OnLeave(string name)
        {
            Console.WriteLine($"{name} just have left");
        }

        public void OnSendMessage(MessageResponse response)
        {
            Console.WriteLine($"{response.UserName} sent {response.Message}");
        }
    }
}
