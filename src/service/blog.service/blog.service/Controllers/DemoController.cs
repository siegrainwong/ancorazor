using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Service.Controllers
{
    [Route("api/[controller]")]
    [Authorize("Admin")]
    [ApiController]
    public class DemoController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        /// <summary>
        /// 恩恩
        /// </summary>
        /// <param name="value">[FromBody]的意思是，你的这个参数需要从Payload做实体序列化</param>
        [HttpPost]
        public void Post([FromBody] Demo value)
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
