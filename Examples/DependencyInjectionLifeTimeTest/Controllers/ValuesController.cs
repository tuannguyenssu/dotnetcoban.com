using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace DependencyInjectionLifeTimeTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        public ITransientService Transient1 { get; }
        public ITransientService Transient2 { get; }
        public IScopedService Scoped1 { get; }
        public IScopedService Scoped2 { get; }
        public ISingletonService Singleton1 { get; }
        public ISingletonService Singleton2 { get; }

        public ValuesController(ITransientService transient1, ITransientService transient2, IScopedService scoped1, IScopedService scoped2, ISingletonService singleton1, ISingletonService singleton2)
        {
            Transient1 = transient1;
            Transient2 = transient2;
            Scoped1 = scoped1;
            Scoped2 = scoped2;
            Singleton1 = singleton1;
            Singleton2 = singleton2;
        }
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            //return new string[] { "value1", "value2" };
            string transient1 = $"Transient 1 : {Transient1.GetID().ToString()}";
            string transient2 = $"Transient 2 : {Transient2.GetID().ToString()}";
            string scoped1 = $"Scoped 1 : {Scoped1.GetID().ToString()}";
            string scoped2 = $"Scoped 2 : {Scoped2.GetID().ToString()}";
            string singleton1 = $"Singleton 1 : {Singleton1.GetID().ToString()}";
            string singleton2 = $"Singleton 2 : {Singleton2.GetID().ToString()}";
            return new string[] { transient1, transient2, scoped1, scoped2, singleton1, singleton2 };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
