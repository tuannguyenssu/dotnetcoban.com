using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GenFu;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreWebApiTest.Controllers
{

    public class Person
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string EmailAddress { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Phone { get; set; }
    }

    [ApiController]
    [Route("/api/[controller]")]
    public class UsersController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<Person>> Get()
        {
            var persons = GenFu.GenFu.ListOf<Person>(25);
            return persons;
        }
    }
}
