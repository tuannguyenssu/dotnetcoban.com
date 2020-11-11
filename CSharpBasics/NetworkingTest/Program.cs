using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace NetworkingTest
{
    class Program
    {
        public static bool ValidateServerCertificate(
            object sender,
            X509Certificate certificate,
            X509Chain chain,
            SslPolicyErrors sslPolicyErrors)
        {
            if (sslPolicyErrors == SslPolicyErrors.None) return true;
            Console.WriteLine("Certificate error: {0}", sslPolicyErrors);

            // Do not allow this client to communicate with unauthenticated servers.
            return false;
        }

        static async Task Main(string[] args)
        {
            TcpClientTest.Run();
            Console.ReadKey();


            //using (var client = new TcpClient())
            //{
            //    var url = "https://xuanthulab.net/robots.txt";
            //    Console.WriteLine($"Start get {url}");
            //    Uri uri = new Uri(url);
            //    var hostAdress = await Dns.GetHostAddressesAsync(uri.Host);
            //    IPAddress ipaddrress = hostAdress[0];
            //    Console.WriteLine($"Host: {uri.Host}, IP: {ipaddrress}");

            //    await client.ConnectAsync(ipaddrress.MapToIPv4(), uri.Port); //kết nối đến HOST
            //    Console.WriteLine("Connected");
            //    Console.WriteLine();


            //    Stream stream; // stream để đọc - gửi HTTP Message
            //    if (uri.Scheme == "https")
            //    {
            //        // SslStream
            //        stream = new SslStream(client.GetStream(), false,
            //            new RemoteCertificateValidationCallback(ValidateServerCertificate),
            //            null);
            //        (stream as SslStream).AuthenticateAsClient(uri.Host); // Xác thực SSL
            //    }
            //    else
            //    {
            //        // NetworkStream
            //        stream = client.GetStream();
            //    }

            //    // Tạo HTTP Request Message (text) và gửi lên server
            //    StringBuilder header = new StringBuilder();
            //    header.Append($"GET {uri.PathAndQuery} HTTP/1.1\r\n");
            //    header.Append($"Host: {uri.Host}\r\n");
            //    header.Append($"\r\n");
            //    byte[] bsend = Encoding.UTF8.GetBytes(header.ToString());
            //    await stream.WriteAsync(bsend, 0, bsend.Length);
            //    await stream.FlushAsync();

            //    Console.WriteLine("Request:");
            //    Console.WriteLine(header);

            //    var ms = new MemoryStream(); // Bộ nhớ lưu dữ liệu tải về
            //    byte[] buffer = new byte[2048];
            //    int bytes = -1;

            //    // Đọc dữ liệu tải về (HTTP Response Message) lưu vào ms
            //    do
            //    {
            //        bytes = await stream.ReadAsync(buffer, 0, buffer.Length);
            //        ms.Write(buffer, 0, bytes);
            //        Array.Clear(buffer, 0, buffer.Length);

            //    } while (bytes > 0);

            //    ms.Seek(0, SeekOrigin.Begin);
            //    var reader = new StreamReader(ms);
            //    string html = reader.ReadToEnd();
            //    Console.WriteLine("Response:");
            //    Console.WriteLine(html);
            //    Console.ReadKey();
            //}
        }
    }
}
