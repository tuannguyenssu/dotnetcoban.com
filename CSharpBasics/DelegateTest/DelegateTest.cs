using System;

namespace DelegateTest
{
    public class DelegateTest
    {
        public delegate void PrintLog(string message);

        public delegate int CalcSum(int a, int b);

        private static void PrintInfoLog(string info)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Info : {info}");
            Console.ResetColor();
        }

        private static void PrintErrorLog(string info)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Info : {info}");
            Console.ResetColor();
        }

        private static int Sum(int a, int b)
        {
            return (a + b);
        }

        public static void Run()
        {
            var log = "test log";
            PrintLog printLog = PrintInfoLog;
            printLog.Invoke(log);

            printLog = PrintErrorLog;
            printLog.Invoke(log);

            CalcSum calcSum = Sum;
            var sum = calcSum.Invoke(10, 20);
            Console.WriteLine(sum);
        }

        public static void RunWithAnonymous()
        {
            var log = "test log";
            PrintLog printLog = delegate(string warning)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"Warning : {warning}");
                Console.ResetColor();
            };

            printLog.Invoke(log);

            CalcSum calcSum = delegate(int a, int b) { return (a + b); };
            var sum = calcSum.Invoke(10, 20);
            Console.WriteLine(sum);
        }

        public static void RunWithLambda()
        {
            var log = "test log";
            PrintLog printLog = warning =>
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"Warning : {warning}");
                Console.ResetColor();
            };

            printLog.Invoke(log);

            CalcSum calcSum = (a, b) => (a + b);
            var sum = calcSum.Invoke(10, 20);
            Console.WriteLine(sum);
        }

        public static void RunWithActionAndFuncAndPredicate()
        {
            var log = "test log";
            Action<string> printLog = warning =>
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"Warning : {warning}");
                Console.ResetColor();
            };

            printLog.Invoke(log);

            Func<int, int, int> calcSum = (a, b) => (a + b);
            var sum = calcSum.Invoke(10, 20);
            Console.WriteLine(sum);

            Predicate<int> isGreaterThanZero = a => (a > 0);
            Console.WriteLine(isGreaterThanZero.Invoke(-10));
            Console.WriteLine(isGreaterThanZero.Invoke(10));
        }
    }
}
