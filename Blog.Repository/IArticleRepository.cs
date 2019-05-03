#region

using Blog.Entity;
using SmartSql.AOP;
using SmartSql.DyRepository;
using SmartSql.DyRepository.Annotations;
using System.Threading.Tasks;

#endregion

namespace Blog.Repository
{
    public interface IArticleRepository : IRepositoryAsync<Article, int>
    {
        [Statement(Id = "GetEntity")]
        Task<Article> GetByIdAsync([Param("Id")] int id, [Param("IsDraft")] bool? isDraft = false);

        [Statement(Id = "GetEntity")]
        Task<Article> GetByAliasAsync([Param("Alias")] string alias, [Param("IsDraft")] bool? isDraft = false);

        [UseTransaction]
        Task<int> DeleteAsync(int id);

        Task<int> InsertAsync(object parameters);

        Task<T> QueryByPageAsync<T>(object parameters);

        Task<int> UpdateAsync(object parameters);

        Task SetArticleCategoriesAsync(int articleId, string[] categories);

        Task SetArticleTagsAsync(int articleId, string[] tags);
    }
}