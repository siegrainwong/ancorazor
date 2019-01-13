//----------BlogArticle开始----------


using System.Collections.Generic;
using System.Threading.Tasks;
using Blog.Common.Attributes;
using Blog.IService.Base;
using Blog.Model;

namespace Blog.IService
{
    /// <summary>
    /// BlogArticleService
    /// </summary>
    public interface IBlogArticleService : IBaseService<BlogArticle>
    {
        [Caching(Expires = 30)]
        Task<List<BlogArticle>> GetBlogs();
    }
}

//----------BlogArticle结束----------
