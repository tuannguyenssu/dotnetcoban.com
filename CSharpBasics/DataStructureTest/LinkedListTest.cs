using System;
using System.Collections.Generic;

namespace DataStructureTest
{
    public class LinkedListTest
    {
        public static void Run()
        {
            var list = new LinkedList<int>();

            list.AddFirst(3);
            list.AddLast(4);
            list.AddFirst(2);
            list.AddFirst(1);

            var listNode = new LinkedListNode<int>(0);

            list.AddBefore(list.First, listNode);

            Console.WriteLine($"Total elements : {list.Count}");
            Console.WriteLine($"First element : {list.First.Value}");
            Console.WriteLine($"Last element : {list.Last.Value}");
            int a = 11;
            Console.WriteLine($"LinkedList contains {a}? : {list.Contains(a)}");

            Console.WriteLine("All elements : ");

            Console.WriteLine("From First -> Last");
            var node = list.First;
            while (node != null)
            {
                Console.WriteLine(node.Value);
                node = node.Next;
            }

            Console.WriteLine("From Last -> First");
            node = list.Last;

            while (node != null)
            {
                Console.WriteLine(node.Value);
                node = node.Previous;
            }
        }
    }
}