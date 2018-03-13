using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace SyntaxAnalysis_api.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("MyPolicy")]
    public class ValuesController : Controller
    {
        private readonly DatabaseContext context = new DatabaseContext();
        // GET api/values
        [HttpGet]
        public string Get()
        {
            return context.Flexes.First().digit;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
