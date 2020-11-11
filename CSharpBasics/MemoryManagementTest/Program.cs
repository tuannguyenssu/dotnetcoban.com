using System;

namespace MemoryManagementTest
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                throw new Exception("test");
                DisposableTest.Run();

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                //throw;
            }
            Console.ReadKey();
        }
    }
}
