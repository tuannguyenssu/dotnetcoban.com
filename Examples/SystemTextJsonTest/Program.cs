using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SystemTextJsonTest
{
    public class Person
    {
        public string Name { get; set; }
        [JsonPropertyName("age")]
        public int? Age { get; set; }
    }
    class Program
    {
        static void Main(string[] args)
        {
            var json = @"{""Name"":""John"", ""age"":""34""}";
            //var obj = JsonDocument.Parse(json);
            //var name = obj.RootElement.GetProperty("Name").GetString();
            //Console.WriteLine(name);

            var options = new JsonSerializerOptions()
            {
                NumberHandling = JsonNumberHandling.AllowReadingFromString
            };

            var person = JsonSerializer.Deserialize<Person>(json, options);
            Console.WriteLine(person.Name);
        }
    }
}
