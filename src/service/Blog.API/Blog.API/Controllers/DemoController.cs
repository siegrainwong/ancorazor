using System.Collections.Generic;
using System.Threading.Tasks;
using Blog.IService;
using Blog.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.API.Controllers
{
    [Route("api/[controller]")]
    //[Authorize("Admin")]
    [AllowAnonymous]
    [ApiController]
    public class DemoController : ControllerBase
    {
        private readonly IArticleService _service;

        public DemoController(IArticleService service)
        {
            _service = service;
        }

        // GET api/values
        [HttpGet]
        public async Task<List<Article>> Get()
        {
            return await _service.GetArticles();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
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
