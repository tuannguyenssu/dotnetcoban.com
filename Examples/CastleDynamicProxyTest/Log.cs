using System;

namespace CastleDynamicProxyTest
{
    public interface ILogger
    {
        public void Log(string message);

        public void ExceptionTest();
    }
    public class Logger : ILogger
    {
        public void Log(string message)
        {
            Console.WriteLine(message);
        }

        public void ExceptionTest()
        {
            throw new Exception("Logger Exception");
        }
    }

    public class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public override string ToString()
        {
            return FirstName + " " + LastName;
        }
    }
}
