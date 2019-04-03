#region

using Blog.Entity;
using SmartSql.DyRepository;
using System.Threading.Tasks;

#endregion

namespace Blog.Repository
{
    public interface IArticleRepository : IRepositoryAsync<Article, int>
    {
        [Statement(Id = "GetEntity")]
        Task<Article> GetByIdAsync([Param("Id")] int id, [Param("IsDeleted")] bool isDeleted = false);

        Task<int> InsertAsync(object parameters);

        Task<T> QueryByPageAsync<T>(object parameters);

        Task<int> UpdateAsync(object parameters);
    }
}