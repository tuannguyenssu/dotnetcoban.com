using System;
using System.Diagnostics;
using System.Text.Json;
using Castle.DynamicProxy;

namespace CastleDynamicProxyTest
{
    public class LoggingInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            var method = invocation.Method;
            var className = method.DeclaringType?.Name;
            var methodName = method.Name;
            var arguments = JsonSerializer.Serialize(invocation.Arguments, new JsonSerializerOptions()
            {
                IgnoreNullValues = true
            });
            try
            {
                Console.WriteLine($"Start calling method: {className}.{methodName} with ({arguments}).");

                var watch = new Stopwatch();
                watch.Start();

                invocation.Proceed();

                watch.Stop();

                Console.WriteLine($"Finished calling method: {className}.{methodName}. Took: {watch.ElapsedMilliseconds} milliseconds");
            }
            catch (Exception e)
            {
                Console.WriteLine($"An unhandled exception has occurred while executing the method: {className}.{methodName} with ({arguments}). {Environment.NewLine}{e}");
                //throw;
            }

        }
    }
}