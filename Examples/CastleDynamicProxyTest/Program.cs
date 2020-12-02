using System;
using Castle.DynamicProxy;

namespace CastleDynamicProxyTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var proxy = new ProxyGenerator();
            var person = proxy.CreateClassProxy<Person>(new LoggingInterceptor());

            person.FirstName = "Tuan";
            person.LastName = "Nguyen";

            Console.WriteLine(person.ToString());


            var logger = proxy.CreateInterfaceProxyWithTarget<ILogger>(new Logger(), new LoggingInterceptor());

            logger.Log(Guid.NewGuid().ToString());
            logger.ExceptionTest();
            Console.ReadKey();

        }
    }
}