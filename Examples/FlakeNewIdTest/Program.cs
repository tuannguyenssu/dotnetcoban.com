using System;
using MassTransit;

namespace FlakeNewIdTest
{
    //https://github.com/phatboyg/NewId
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(NewId.Next());
            //Similar to GUID
            Console.WriteLine(NewId.Next().ToString("D").ToUpperInvariant());
            Console.ReadKey();
        }
    }
}
