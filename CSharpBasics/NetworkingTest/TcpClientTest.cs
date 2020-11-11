using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace NetworkingTest
{
    public class TcpClientTest
    {
        public static async void Run()
        {
            var url = "https://www.dotnetcoban.com";
            //var url = "https://xuanthulab.net/robots.txt";

            using var client = new TcpClient();
            var uri = new Uri(url);

            var hostAddress = Dns.GetHostAddresses(uri.Host)[0];

            Console.WriteLine($"Host: {uri.Host}, IP: {hostAddress}");

            await client.ConnectAsync(hostAddress.MapToIPv4(), uri.Port);

            var stream = new SslStream(client.GetStream(), false, new RemoteCertificateValidationCallback(ValidateServerCertificate), null);
            stream.AuthenticateAsClient(uri.Host);

            var request = new StringBuilder().Append($"GET {uri.PathAndQuery} HTTP/1.1\r\n").Append($"Host: {uri.Host}\r\n").Append("\r\n").ToString();

            await stream.WriteAsync(Encoding.UTF8.GetBytes(request), 0, request.Length);

            await stream.FlushAsync();

            Console.WriteLine("Request:");
            Console.WriteLine(request);

            var ms = new MemoryStream();
            byte[] buffer = new byte[2048];
            int bytes = -1;

            do
            {
                bytes = await stream.ReadAsync(buffer, 0, buffer.Length);
                ms.Write(buffer, 0, bytes);
                Array.Clear(buffer, 0, buffer.Length);

            } while (bytes > 0);

            ms.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(ms);
            string response = reader.ReadToEnd();
            Console.WriteLine("Response:");
            Console.WriteLine(response);

        }

        private static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslpolicyerrors)
        {
            if (sslpolicyerrors == SslPolicyErrors.None) return true;
            Console.WriteLine("Certificate error: {0}", sslpolicyerrors);

            // Do not allow this client to communicate with unauthenticated servers.
            return false;
        }
    }
}
