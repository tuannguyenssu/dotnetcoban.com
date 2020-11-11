using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreadingTest
{
    public class ParallelProgrammingTest
    {
        public static void RunParallelFor()
        {
            var startIndex = 0;
            var stopIndex = 100;
            Action<int> task = (index) =>
            {
                var taskInfo = $"Current Index: {index}\t Task Id: {Task.CurrentId}\t Thread Id: {Thread.CurrentThread.ManagedThreadId}";
                Console.WriteLine(taskInfo);
            };
            var result = Parallel.For(startIndex, stopIndex, task);
            Console.WriteLine($"All tasks finished? {result.IsCompleted}");
        }

        public static void RunParallelForEach()
        {
            var items = new List<string>();
            items.Add("Tuan Nguyen");
            items.Add("Nguyen Tuan");
            items.Add("Nguyen Van Tuan");

            Action<string> task = (s) => { Console.WriteLine(s); };

            var result = Parallel.ForEach(items, task);

            Console.WriteLine($"All tasks finished? {result.IsCompleted}");
        }

        public static void RunParallelInvoke()
        {
            Action<string> downloadTask = async (url) =>
            {
                using WebClient client = new WebClient();
                var s = await client.DownloadStringTaskAsync(url);
                Console.WriteLine($"{url} : {s.Length}");
            };

            Action downloadWebsite1 = () => downloadTask("https://www.dotnetcoban.com");
            Action downloadWebsite2 = () => downloadTask("https://www.google.com.vn");
            Action downloadWebsite3 = () => downloadTask("https://www.facebook.com");
            Parallel.Invoke(downloadWebsite1, downloadWebsite2, downloadWebsite3);
        }
    }
}
