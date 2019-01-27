#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Common.Redis;
using Blog.IService;
using Blog.Model;
using Blog.Model.ViewModel;
using Blog.Model.ViewModel.ParameterModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

#endregion

namespace Blog.API.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    //[Authorize(Policy = "Admin")]
    [ApiController]
    public class BlogsController : ControllerBase
    {
        private IArticleService _service;
        private IRedisCacheManager _cache;
        public BlogsController(IArticleService service, IRedisCacheManager cache)
        {
            _service = service;
            _cache = cache;
        }

        /// <summary>
        /// 获取博客列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<object> Get([FromQuery]QueryParameters parameters)
        {
            const int pageSize = 6;
            List<Article> list;

            const string key = "Redis.Article";

            if (_cache.Get<object>(key) != null)
            {
                list = _cache.Get<List<Article>>(key);
            }
            else
            {
                list = await _service.Query();

                _cache.Set(key, list, TimeSpan.FromHours(2));
            }
            var pageCount = list.Count / pageSize;

            list = list.OrderByDescending(d => d.Id).Skip(parameters.PageIndex * pageSize).Take(parameters.PageSize).ToList();

            foreach (var item in list)
            {
                if (string.IsNullOrEmpty(item.Content)) continue;

                const int totalLength = 500;
                if (item.Content.Length > totalLength) item.Content = item.Content.Substring(0, totalLength);
            }

            return new { succeed = true, data = new { list, parameters.PageNumber, pageCount } };
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