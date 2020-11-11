using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace MultiThreadingTest
{
    public class ConcurrentCollectionTest
    {
        public static void RunConcurrentStack()
        {
            var concurrent = new ConcurrentStack<int>();
            for (var i = 0; i < 10; i++)
            {
                concurrent.Push(i);
            }

            while (!concurrent.IsEmpty)
            {
                if (concurrent.TryPop(out var item))
                {
                    Console.WriteLine(item);
                }
            }
        }

        public static void RunConcurrentQueue()
        {
            var concurrent = new ConcurrentQueue<int>();
            for (var i = 0; i < 10; i++)
            {
                concurrent.Enqueue(i);
            }

            while (!concurrent.IsEmpty)
            {
                if (concurrent.TryDequeue(out var item))
                {
                    Console.WriteLine(item);
                }
            }
        }

        // Luu khong theo thu tu
        public static void RunConcurrentBag()
        {
            var concurrent = new ConcurrentBag<int>();
            for (var i = 0; i < 10; i++)
            {
                concurrent.Add(i);
            }

            while (!concurrent.IsEmpty)
            {
                if (concurrent.TryTake(out var item))
                {
                    Console.WriteLine(item);
                }
            }
        }

        public static void RunBlockingCollection()
        {
            var concurrent = new BlockingCollection<int>();
            for (var i = 0; i < 10; i++)
            {
                concurrent.Add(i);
            }

            while (concurrent.Count > 0)
            {
                if (concurrent.TryTake(out var item))
                {
                    Console.WriteLine(item);
                }
            }
        }

        public static void RunConcurrentDictionary()
        {
            var concurrent = new ConcurrentDictionary<int, string>();
            for (var i = 0; i < 10; i++)
            {
                concurrent.TryAdd(i, $"{i}");
            }

            foreach (var item in concurrent)
            {
                Console.WriteLine($"{item.Key} {item.Value}");
            }
        }
    }
}
