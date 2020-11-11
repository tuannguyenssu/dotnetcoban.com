using System;
using System.Collections.Generic;

namespace DataStructureTest
{
    public class StackTest
    {
        public static void Run()
        {
            var stack = new Stack<int>();
            for (int i = 0; i < 10; i++)
            {
                stack.Push(i);
            }

            int a = 11;
            Console.WriteLine($"Stack contains {a}? : {stack.Contains(a)}");

            Console.WriteLine($"First element :");
            Console.WriteLine(stack.Peek());
            Console.WriteLine("All elements : ");
            while (stack.Count > 0)
            {
                Console.WriteLine(stack.Pop());
            }
        }
    }
}