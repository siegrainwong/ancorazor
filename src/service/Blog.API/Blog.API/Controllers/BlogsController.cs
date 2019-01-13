using System.Collections.Generic;
using System.Threading.Tasks;
using Blog.IService;
using Blog.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.API.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    [ApiController]
    public class BlogsController : ControllerBase
    {
        private IBlogArticleService _service;
        public BlogsController(IBlogArticleService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<List<BlogArticle>> Get()
        {
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