using System;
using System.Text.Json;
using GenFu;

namespace GenFuTest
{
    //https://github.com/MisterJames/GenFu
    class Program
    {
        static void Main(string[] args)
        {
            var options = new JsonSerializerOptions()
            {
                WriteIndented = true
            };

            GenFu.GenFu.Configure<Person>().Fill(p => p.Age).WithinRange(50, 80);
            GenFu.GenFu.Configure<Person>().Fill(p => p.Title, () => { return "Sir/Madam";} );
            GenFu.GenFu.Configure<Person>().MethodFill<string>(p => p.SetMiddleName(null));

            var person = A.New<Person>();
            Console.WriteLine(JsonSerializer.Serialize(person, options));

            var persons = A.ListOf<Person>(100);
            Console.WriteLine(JsonSerializer.Serialize(persons, options));
            Console.ReadKey();
        }
    }
}
