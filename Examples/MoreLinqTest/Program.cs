using System;
using System.Collections.Generic;
using MoreLinq.Extensions;

namespace MoreLinqTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var names = new List<string>()
            {
                "Tuan",
                "Tuan"
            };

            var atLeast = names.AtLeast(2);
            Console.WriteLine(atLeast);
            Console.ReadKey();
        }
    }
}
