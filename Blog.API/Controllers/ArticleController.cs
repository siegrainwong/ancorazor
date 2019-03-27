#region

using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Blog.API.Messages;
using Blog.API.Messages.Article;
using Blog.API.Messages.Users;
using Blog.Entity;
using Blog.Repository;
using Blog.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

#endregion

namespace Blog.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ArticleController : ControllerBase
    {
        private IArticleRepository _repository;
        private ArticleService _service;
        private CategoryService _categoryService;

        public ArticleController(ArticleService service, IArticleRepository repository, CategoryService categoryService)
        {
            _service = service;
            _repository = repository;
            _categoryService = categoryService;
        }

        [HttpPost]
        public async Task<IActionResult> Insert([FromBody] [Required] ArticleUpdateParameter parameter)
        {
            // TODO: add tags\categories supporting
            return CreatedAtAction(nameof(Get), new {id = parameter.Id},
                new ResponseMessage<int> {Data = await _repository.InsertAsync(parameter)});
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] [Required] ArticleUpdateParameter parameter)
        {
            var result = await _service.UpdateAsync(parameter);
            return Ok(new ResponseMessage<int> {Succeed = result, Data = parameter.Id});
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([Range(1, int.MaxValue)] int id)
        {
            var result = await _repository.DeleteByIdAsync(id);
            return Ok(new ResponseMessage<int> {Succeed = result > 0, Data = id});
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([Range(1, int.MaxValue)] int id)
        {
            var article = await _repository.GetByIdAsync(id);
            if (article == null) return NotFound();
            return Ok(new ResponseMessage<Article> {Data = article});
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetPaged([FromQuery] [Required] QueryByPageParameter parameters)
        {
            var result = await _service.QueryByPageAsync(parameters);
            return Ok(new ResponseMessage<QueryByPageResponse<Article>> {Data = result});
        }
    }
}