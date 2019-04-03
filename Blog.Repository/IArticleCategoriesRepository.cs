#region

using Blog.Entity;
using SmartSql.DyRepository;

#endregion

namespace Blog.Repository
{
    public interface IArticleCategoriesRepository : IRepositoryAsync<ArticleCategories, int>
    {
        //Task<int> DeleteByArticleAsync(int articleId);
        //Task<int> InsertBatchAsync(IEnumerable<ArticleCategories> articleCategories);
    }
}