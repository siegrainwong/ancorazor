using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Blog.IService;
using Blog.Model;
using Blog.Model.ViewModel;
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
        private readonly IMapper _mapper;
        private readonly IArticleService _service;

        public DemoController(
            IMapper mapper,
            IArticleService service)
        {
            _service = service;
            _mapper = mapper;
        }

        // GET api/values
        [HttpGet]
        public async Task<List<Article>> Get()
        {
            var result = _mapper.Map(new Article(), new ArticleViewModel());

            //return null;
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
