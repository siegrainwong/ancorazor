#region

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blog.API.Caching;
using Blog.Common.Configuration;
using Blog.Common.Redis;
using Blog.IService;
using Blog.Model;
using Blog.Model.ViewModel;
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
        IBlogArticleService _service;
        IRedisCacheManager _cache;
        public BlogsController(IBlogArticleService service, IRedisCacheManager cache)
        {
            _service = service;
            _cache = cache;
        }

        [HttpGet]
        public async Task<List<BlogArticle>> Get()
        {
            List<BlogArticle> result;
            const string key = "Blog.GetArticles";
            if (_cache.Get<object>(key) != null)
            {
                result = _cache.Get<List<BlogArticle>>(key);
            }
            else
            {
                result = await _service.GetArticles();
                _cache.Set(key, result, TimeSpan.FromHours(2));
            }
            return result;
        }

        [HttpGet("{id}", Name = "Get")]
        public async Task<BlogViewModel> Get(int id)
        {
            return await _service.GetArticle(id);
        }
    }
}