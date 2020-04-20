using System;
using System.Net;

namespace IPNetwork2Test
{
    //https://github.com/lduchosal/ipnetwork
    class Program
    {
        static void Main(string[] args)
        {
            IPNetwork ipnetwork = IPNetwork.Parse("2001:0db8::/64");

            Console.WriteLine("Network : {0}", ipnetwork.Network);
            Console.WriteLine("Netmask : {0}", ipnetwork.Netmask);
            Console.WriteLine("Broadcast : {0}", ipnetwork.Broadcast);
            Console.WriteLine("FirstUsable : {0}", ipnetwork.FirstUsable);
            Console.WriteLine("LastUsable : {0}", ipnetwork.LastUsable);
            Console.WriteLine("Usable : {0}", ipnetwork.Usable);
            Console.WriteLine("Cidr : {0}", ipnetwork.Cidr);
            Console.ReadKey();
        }
    }
}
