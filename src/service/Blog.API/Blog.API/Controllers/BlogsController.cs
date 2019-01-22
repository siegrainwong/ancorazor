#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Common.Redis;
using Blog.IService;
using Blog.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

#endregion

namespace Blog.API.Controllers
{
    [Route("api/[controller]")]
    //[AllowAnonymous]
    [Authorize(Policy = "Admin")]
    [ApiController]
    public class BlogsController : ControllerBase
    {
        private IBlogArticleService _service;
        private IRedisCacheManager _cache;
        public BlogsController(IBlogArticleService service, IRedisCacheManager cache)
        {
            _service = service;
            _cache = cache;
        }

        /// <summary>
        /// 获取博客列表
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pageNumber"></param>
        /// <param name="category"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<object> Get(int id, int pageNumber, string category)
        {
            const int pageSize = 6;
            List<BlogArticle> list;

            if (_cache.Get<object>("Redis.Blog") != null)
            {
                list = _cache.Get<List<BlogArticle>>("Redis.Blog");
            }
            else
            {
                list = string.IsNullOrEmpty(category)
                    ? await _service.Query()
                    : await _service.Query(a => a.Category == category);

                _cache.Set("Redis.Blog", list, TimeSpan.FromHours(2));
            }
            var pageCount = list.Count / pageSize;

            list = list.OrderByDescending(d => d.Id).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            foreach (var item in list)
            {
                if (string.IsNullOrEmpty(item.Content)) continue;

                const int totalLength = 500;
                if (item.Content.Length > totalLength) item.Content = item.Content.Substring(0, totalLength);
            }

            return new { succeed = true, data = new { list, pageNumber, pageCount } };
        }

        // GET: api/Blog/5
        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "Get")]
        public async Task<object> Get(int id)
        {
            var model = await _service.GetArticle(id);
            return new { succeed = true, data = model };
        }
    }
}