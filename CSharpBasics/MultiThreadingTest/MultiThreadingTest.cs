using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace MultiThreadingTest
{
    public class MultiThreadingTest
    {
        public static void RunSimpleThread()
        {
            var thread = new Thread(BackgroundJob);
            thread.Start();
            Console.WriteLine("Main Thread");
            thread.Join();
            Console.WriteLine("Background Thread Completed!");
        }


        public static void RunSimpleThreadPool()
        {
            ThreadPool.QueueUserWorkItem(BackgroundJob);
            Console.WriteLine("Main Thread");
            Console.WriteLine("Background Thread Completed!");
        }

        public static void RunAsynchronousDelegates()
        {

        }

        public static void RunBackgroundWorker()
        {

        }

        private static void BackgroundJob(object? state)
        {
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine($"Background Thread {i}");
                Thread.Sleep(1000);
            }
        }
    }
}
