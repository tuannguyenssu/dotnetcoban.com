using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace PerformanceCounterTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var cpu = GetProcessCPU();
            var memory = GetProcessMemory();
            var gcHeap = GetGCHeapMemory();
            var threads = GetThreadCount();

            Console.WriteLine($"{cpu} {memory} {gcHeap} {threads}");
            Console.ReadKey();
        }

        private static double GetProcessMemory()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                var counter = new PerformanceCounter("Process", "Working Set - Private", Process.GetCurrentProcess().ProcessName);
                var memory = counter.NextValue() / 1024.00 / 1024.00;
                return memory;
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                return Process.GetCurrentProcess().WorkingSet64 / 1024.00 / 1024.00;
            }

            return 0;
        }
        
        private static double _prevCpuTime;


        private static double GetProcessCPU()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                var counter = new PerformanceCounter("Process", "% Processor Time", Process.GetCurrentProcess().ProcessName);
                return counter.NextValue() / Convert.ToDouble(Environment.ProcessorCount);
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                var process = Process.GetCurrentProcess();

                var curTime = process.TotalProcessorTime.TotalMilliseconds;

                var value = (curTime - _prevCpuTime) / Convert.ToDouble(1) / Convert.ToDouble(Environment.ProcessorCount) * 100.00;

                _prevCpuTime = curTime;

                return value;
            }

            return 0;
        }
        

        public static double GetGCHeapMemory()
        {
            return GC.GetTotalMemory(false) / 1024.00 / 1024.00;
        }

        private static int GetThreadCount()
        {
            ThreadPool.GetMaxThreads(out int max, out _);

            ThreadPool.GetAvailableThreads(out int current, out _);

            return max - current;
        }
    }
}
