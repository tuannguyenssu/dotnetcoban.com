using System;
using System.Linq;
using MoreLinq;

namespace MoreLinqTest
{
    //https://github.com/morelinq/MoreLINQ
    class Program
    {
        static void Main(string[] args)
        {
            var numbers = Enumerable.Range(1, 100);
            Console.WriteLine(numbers.AtLeast(1));
            Console.WriteLine(numbers.AtMost(0));
            foreach (var batch in numbers.Batch(10))
            {
                foreach (var number in batch)
                {
                    Console.Write($"{number} ");
                }
                Console.WriteLine();
            }

            Console.ReadKey();
        }
    }
}
