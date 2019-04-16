#region

using Blog.API.Messages;
using Blog.API.Messages.Article;
using Blog.Entity;
using Blog.Repository;
using Blog.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

#endregion

namespace Blog.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArticleController : ControllerBase
    {
        private readonly IArticleRepository _repository;
        private readonly ArticleService _service;

        public ArticleController(ArticleService service, IArticleRepository repository)
        {
            _service = service;
            _repository = repository;
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var article = await _repository.GetByIdAsync(id);
            if (article == null) return NotFound();
            return Ok(new ResponseMessage<Article> { Data = article });
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetPaged([FromQuery] QueryByPageParameter parameters)
        {
            var result = await _service.QueryByPageAsync(parameters);
            return Ok(new ResponseMessage<QueryByPageResponse<Article>> { Data = result });
        }

        [HttpPost]
        public async Task<IActionResult> Insert([FromBody] ArticleUpdateParameter parameter)
        {
            var result = await _service.InsertAsync(parameter);
            return CreatedAtAction(nameof(Get), new { id = result }, new ResponseMessage<int> { Data = result });
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] ArticleUpdateParameter parameter)
        {
            var result = await _service.UpdateAsync(parameter);
            return Ok(new ResponseMessage<int> { Succeed = result, Data = parameter.Id });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _repository.DeleteAsync(id);
            return Ok(new ResponseMessage<int> { Succeed = result > 0, Data = id });
        }
    }
}