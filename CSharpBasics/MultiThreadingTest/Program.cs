using System;

namespace MultiThreadingTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //MultiThreadingTest.RunSimpleThread();
            //MultiThreadingTest.RunSimpleThreadPool();


            //ConcurrentCollectionTest.RunConcurrentStack();
            //ConcurrentCollectionTest.RunConcurrentQueue();
            //ConcurrentCollectionTest.RunConcurrentBag();
            //ConcurrentCollectionTest.RunBlockingCollection();
            //ConcurrentCollectionTest.RunConcurrentDictionary();

            //ParallelProgrammingTest.RunParallelFor();
            //ParallelProgrammingTest.RunParallelForEach();
            //ParallelProgrammingTest.RunParallelInvoke();

            //TimerTest.RunSystemThreadingTimer();
            TimerTest.RunSystemTimersTimer();
            Console.ReadKey();
        }
    }
}
