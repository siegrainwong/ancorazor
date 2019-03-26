#region

using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Blog.API.Messages;
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

        public ArticleController(ArticleService service, IArticleRepository repository)
        {
            _service = service;
            _repository = repository;
        }

        [HttpPost]
        public async Task<IActionResult> Insert([FromBody] [Required] ArticleParameter parameters)
        {
            // TODO: add tags\categories supporting
            parameters.Author = 1;
            parameters.CreatedAt = DateTime.Now;
            parameters.UpdatedAt = DateTime.Now;
            return CreatedAtAction(nameof(Get), new {id = parameters.Id},
                new ResponseMessage<int> {Data = await _repository.InsertAsync(parameters)});
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] [Required] ArticleParameter parameters)
        {
            parameters.Author = 1;
            parameters.UpdatedAt = DateTime.Now;
            var result = await _repository.UpdateAsync(parameters);
            return Ok(new ResponseMessage<int> {Succeed = result > 0, Data = parameters.Id});
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