using System;
using System.Globalization;
using System.Threading;
using Humanizer;

namespace HumanizerTest
{
    //https://github.com/Humanizr/Humanizer
    class Program
    {
        static void Main(string[] args)
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("vi-VN");
            Console.WriteLine("PascalCaseInputStringIsTurnedIntoSentence".Humanize());
            Console.WriteLine(DateTime.UtcNow.AddHours(-30).Humanize());
            Console.ReadKey();
        }
    }
}
