using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;

namespace SignalRTest.Shared
{
    public class MessageReceivedEventArgs : EventArgs
    {
        public MessageReceivedEventArgs(string username, string message)
        {
            Username = username;
            Message = message;
        }

        public string Username { get; set; }

        public string Message { get; set; }

    }

    public class ChatClient : IAsyncDisposable
    {
        public const string HubUrl = "/ChatHub";
        
        public const string ReceiveMethodName = "ReceiveMessage";
        public const string SendMethodName = "SendMessage";
        public const string RegisterMethodName = "Register";

        private HubConnection _hubConnection;
        private readonly string _hubUrl;
        private readonly string _userName;

        private bool _started = false;
        public delegate void MessageReceivedEventHandler(object sender, MessageReceivedEventArgs e);
        public event MessageReceivedEventHandler MessageReceived;

        public ChatClient(string userName, string siteUrl)
        {
            if (string.IsNullOrWhiteSpace(userName))
                throw new ArgumentNullException(nameof(userName));
            if (string.IsNullOrWhiteSpace(siteUrl))
                throw new ArgumentNullException(nameof(siteUrl));
            _userName = userName;
            _hubUrl = siteUrl.TrimEnd('/') + HubUrl;
        }

        public async Task StartAsync()
        {
            if (!_started)
            {
                _hubConnection = new HubConnectionBuilder()
                    .WithUrl(_hubUrl)
                    .Build();
                //Console.WriteLine("ChatClient: calling Start()");

                _hubConnection.On<string, string>(ReceiveMethodName, (user, message) =>
                {
                    MessageReceived?.Invoke(this, new MessageReceivedEventArgs(user, message));
                });

                await _hubConnection.StartAsync();

                //Console.WriteLine("ChatClient: Start returned");
                _started = true;

                await _hubConnection.SendAsync(RegisterMethodName, _userName);
            }
        }

        public async Task StopAsync()
        {
            if (_started)
            {
                await _hubConnection.StopAsync();
                await _hubConnection.DisposeAsync();
                _hubConnection = null;
                _started = false;
            }
        }

        public async ValueTask DisposeAsync()
        {
            //Console.WriteLine("ChatClient: Disposing");
            await StopAsync();
        }

        public async Task SendAsync(string message)
        {
            if (!_started)
                throw new InvalidOperationException("Client not started");
            await _hubConnection.SendAsync(SendMethodName, _userName, message);
        }
    }
}
