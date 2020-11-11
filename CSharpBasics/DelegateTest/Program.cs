using System;

namespace DelegateTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //DelegateTest.Run();
            //DelegateTest.RunWithAnonymous();
            //DelegateTest.RunWithLambda();
            DelegateTest.RunWithActionAndFuncAndPredicate();
            Console.ReadKey();
        }
    }
}
