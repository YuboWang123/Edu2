using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.AspNetCore.Mvc;

namespace Edu.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly ConfigurationDbContext _context;

        public ValuesController(ConfigurationDbContext cxt)
        {
            _context = cxt;
        }


        #region MyRegion

        /// <summary>
        /// add new client
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Post([FromBody] IdentityServer4.EntityFramework.Entities.Client client)
        {
            var res = _context.Clients.Add(client);
            if (_context.SaveChanges() > 0)
                return Ok(true);
            else
                return Ok(false);
        }


        #endregion


        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
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
