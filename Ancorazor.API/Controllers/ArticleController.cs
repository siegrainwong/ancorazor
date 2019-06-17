#region

using Ancorazor.API.Controllers.Base;
using Ancorazor.API.Messages;
using Ancorazor.API.Messages.Article;
using Ancorazor.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

#endregion

namespace Ancorazor.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArticleController : SGControllerBase
    {
        private readonly ArticleService _service;

        public ArticleController(ArticleService service, UserService userService) : base(userService)
        {
            _service = service;
        }

        [AllowAnonymous]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var article = await _service.GetByIdAsync(id, IsAuthenticated ? (bool?)null : false);
            if (article == null) return NotFound();
            return Ok(article);
        }

        [AllowAnonymous]
        [HttpGet("{alias}")]
        public async Task<IActionResult> GetByAlias(string alias)
        {
            var article = await _service.GetByAliasAsync(alias, IsAuthenticated ? (bool?)null : false);
            if (article == null) return NotFound();
            return Ok(article);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetPaged([FromQuery] ArticlePaginationParameter parameters)
        {
            if (HttpContext.User.Identity.IsAuthenticated) parameters.IsDraft = null;
            return Ok(await _service.QueryByPageAsync(parameters));
        }

        [HttpPost]
        public async Task<IActionResult> Insert([FromBody] ArticleUpdateParameter parameter)
        {
            return Ok(await _service.UpsertAsync(parameter));
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] ArticleUpdateParameter parameter)
        {
            return Ok(await _service.UpsertAsync(parameter));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(succeed: await _service.DeleteAsync(id), data: id);
        }
    }
}