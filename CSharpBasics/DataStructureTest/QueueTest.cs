using System;
using System.Collections.Generic;

namespace DataStructureTest
{
    public class QueueTest
    {
        public static void Run()
        {
            var queue = new Queue<int>();
            for (int i = 0; i < 10; i++)
            {
                queue.Enqueue(i);
            }

            int a = 11;
            Console.WriteLine($"Queue contains {a}? : {queue.Contains(a)}");

            Console.WriteLine($"First element :");
            Console.WriteLine(queue.Peek());
            Console.WriteLine("All elements : ");
            while (queue.Count > 0)
            {
                Console.WriteLine(queue.Dequeue());
            }
        }
    }
}
