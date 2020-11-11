using System;

namespace ExtensionMethodTest
{
    public static class StringExtension
    {
        public static void PrintAsError(this string s)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(s);
        }

        public static void PrintAsWarning(this string s)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(s);
        }
    }

    public class ExtensionMethodTest
    {
        public static void Run()
        {
            var s = "Error Message";
            s.PrintAsError();
            s = "Warning Message";
            s.PrintAsWarning();
        }
    }
}
