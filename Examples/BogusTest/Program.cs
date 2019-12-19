using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Bogus;
using Bogus.Extensions;
using static Bogus.DataSets.Name;

namespace BogusTest
{
    public class Order
    {
        public int OrderId { get; set; }
        public int Quantity { get; set; }
        public int LotNumber { get; set; }
        public string Item { get; set; }
    }

    public class User
    {
        public User(int userId, string ssn)
        {
            this.Id = userId;
            this.SSN = ssn;
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string SomethingUnique { get; set; }
        public Guid SomeGuid { get; set; }

        public string Avatar { get; set; }
        public Guid CartId { get; set; }
        public string SSN { get; set; }
        public Gender Gender { get; set; }
    }

    //https://github.com/bchavez/Bogus
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            var options = new JsonSerializerOptions()
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };

            //Set the randomizer seed if you wish to generate repeatable data sets.
            Randomizer.Seed = new Random(8675309);

            var fruit = new[] { "apple", "banana", "orange", "strawberry", "kiwi" };

            var orderIds = 0;
            var testOrders = new Faker<Order>()
                //Ensure all properties have rules. By default, StrictMode is false
                //Set a global policy by using Faker.DefaultStrictMode
                .StrictMode(true)
                //OrderId is deterministic
                .RuleFor(o => o.OrderId, f => orderIds++)
                //Pick some fruit from a basket
                .RuleFor(o => o.Item, f => f.PickRandom(fruit))
                //A random quantity from 1 to 10
                .RuleFor(o => o.Quantity, f => f.Random.Number(1, 10))
                //A nullable int? with 80% probability of being null.
                //The .OrNull extension is in the Bogus.Extensions namespace.
                .RuleFor(o => o.LotNumber, f => f.Random.Int(0, 100).OrNull(f, .8f));

            //Console.WriteLine(JsonSerializer.Serialize(testOrders.Generate(3), options));

            var userIds = 0;
            var testUsers = new Faker<User>()
                //Optional: Call for objects that have complex initialization
                .CustomInstantiator(f => new User(userIds++, f.Random.Replace("###-##-####")))

                //Use an enum outside scope.
                .RuleFor(u => u.Gender, f => f.PickRandom<Gender>())

                //Basic rules using built-in generators
                .RuleFor(u => u.FirstName, (f, u) => f.Name.FirstName(u.Gender))
                .RuleFor(u => u.LastName, (f, u) => f.Name.LastName(u.Gender))
                .RuleFor(u => u.Avatar, f => f.Internet.Avatar())
                .RuleFor(u => u.UserName, (f, u) => f.Internet.UserName(u.FirstName, u.LastName))
                .RuleFor(u => u.Email, (f, u) => f.Internet.Email(u.FirstName, u.LastName))
                .RuleFor(u => u.SomethingUnique, f => $"Value {f.UniqueIndex}")

                //Use a method outside scope.
                .RuleFor(u => u.CartId, f => Guid.NewGuid())
                //Compound property with context, use the first/last name properties
                .RuleFor(u => u.FullName, (f, u) => u.FirstName + " " + u.LastName)
                //And composability of a complex collection.
                //Optional: After all rules are applied finish with the following action
                .FinishWith((f, u) =>
                {
                    Console.WriteLine("User Created! Id={0}", u.Id);
                });

            var user = testUsers.Generate();
            Console.WriteLine(JsonSerializer.Serialize(user, options));

            var faker = new Faker("vi");
            var o = new Order()
            {
                OrderId = faker.Random.Number(1, 100),
                Item = faker.Lorem.Sentence(),
                Quantity = faker.Random.Number(1, 10)
            };



            Console.WriteLine(JsonSerializer.Serialize(o, options));

            Console.ReadKey();
        }
    }
}
