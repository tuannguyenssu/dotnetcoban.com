using System;
using System.IO;

namespace MemoryManagementTest
{
    //https://docs.microsoft.com/en-us/dotnet/api/system.idisposable

    public class FileWriter : IDisposable
    {
        private bool _disposed = false;

        private readonly StreamWriter _streamWriter;

        public FileWriter(string fileName)
        {
            _streamWriter = new StreamWriter(fileName, true);
        }

        public void Write(string text)
        {
            _streamWriter.WriteLine(text);
            Console.WriteLine($"Written {text}");
        }


        public void Dispose()
        {
            Console.WriteLine($"{nameof(FileWriter)} disposed");
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _streamWriter.Dispose();
                }
                _disposed = true;
            }
        }

        ~FileWriter()
        {
            Dispose(false);
        }
    }
    public class DisposableTest
    {
        public static void Run()
        {
            var fileWriter1 = new FileWriter("test.txt");
            fileWriter1.Write("dotnetcoban.com");
            fileWriter1.Dispose();

            using (var fileWriter2 = new FileWriter("test.txt"))
            {
                fileWriter2.Write("dotnetcoban.com");
            }
        }
    }


}
