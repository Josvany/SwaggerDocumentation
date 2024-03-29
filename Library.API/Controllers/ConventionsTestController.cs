﻿using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Library.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiConventionType(typeof(DefaultApiConventions))]
    public class ConventionsTestController : ControllerBase
    {
        // GET: api/<ConventionsTestController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<ConventionsTestController>/5
        [HttpGet("{id}")]
        //[ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ConventionsTestController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ConventionsTestController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ConventionsTestController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
