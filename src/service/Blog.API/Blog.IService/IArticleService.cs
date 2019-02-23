#region

using System.Collections.Generic;
using System.Threading.Tasks;
using Blog.Common.Attributes;
using Blog.IService.Base;
using Blog.Model;
using Blog.Model.ParameterModel;
using Blog.Model.Resources;
using Blog.Model.ViewModel;

#endregion

namespace Blog.IService
{	
	/// <summary>
	/// ArticleService
	/// </summary>	
    public interface IArticleService :IBaseService<Article>
	{
        [Caching(Expires = 30)]
        Task<List<Article>> GetArticles();
        Task<Article> GetArticle(int id);
        Task<PaginatedList<Article>> GetPagedArticles(ArticleParameters parameters);
    }
}
	