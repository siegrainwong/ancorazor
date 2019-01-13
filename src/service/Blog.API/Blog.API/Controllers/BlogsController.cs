#region

using System.Collections.Generic;
using System.Threading.Tasks;
using Blog.API.Caching;
using Blog.Common.Redis;
using Blog.IService;
using Blog.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

#endregion

namespace Blog.API.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    [ApiController]
    public class BlogsController : ControllerBase
    {
        private IBlogArticleService _service;
        IRedisCacheManager _cache;
        public BlogsController(IBlogArticleService service, IRedisCacheManager redisCacheManager)
        {
            _service = service;
            this._cache = redisCacheManager;
        }

        [HttpGet]
        public async Task<List<BlogArticle>> Get()
        {
            // TODO: 作者要在这里用redis
            return await _service.GetBlogs();
        }

        [HttpGet]
        [Route("/id")]
        public async Task<BlogArticle> Get(int id)
        {
            return await _service.QueryByID(id);
        }
    }
}