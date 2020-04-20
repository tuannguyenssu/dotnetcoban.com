using System;
using Ardalis.GuardClauses;
using Dawn;

namespace GuardTest
{
    //https://github.com/ardalis/guardclauses
    //https://github.com/safakgur/guard
    class Program
    {
        static void Main(string[] args)
        {
            //TestGuardClauses();
            TestGuard();
            Console.ReadKey();
        }

        static void TestGuard()
        {
            var test = new string("test");
            Dawn.Guard.Argument(test, nameof(test)).NotNull().NotEmpty();
            Console.WriteLine("Not null");
            test = null;
            Dawn.Guard.Argument(test, nameof(test)).NotNull().NotEmpty();
        }

        static void TestGuardClauses()
        {
            var test = new string("test");
            Ardalis.GuardClauses.Guard.Against.Null(test, nameof(test));
            Console.WriteLine("Not null");

            test = null;
            Ardalis.GuardClauses.Guard.Against.Null(test, nameof(test));
        }
    }
}
