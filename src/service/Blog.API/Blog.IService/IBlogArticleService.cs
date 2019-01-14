//----------BlogArticle开始----------


using System.Collections.Generic;
using System.Threading.Tasks;
using Blog.Common.Attributes;
using Blog.IService.Base;
using Blog.Model;
using Blog.Model.ViewModel;

namespace Blog.IService
{
    /// <summary>
    /// BlogArticleService
    /// </summary>
    public interface IBlogArticleService : IBaseService<BlogArticle>
    {
        [Caching(Expires = 30)]
        Task<List<BlogArticle>> GetArticles();
        Task<BlogViewModel> GetArticle(int id);
    }
}

//----------BlogArticle结束----------
