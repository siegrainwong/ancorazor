#region

using Blog.Entity;
using SmartSql.DyRepository;
using SmartSql;
using System.Threading.Tasks;
using SmartSql.DyRepository.Annotations;

#endregion

namespace Blog.Repository
{
    public interface IArticleRepository : IRepositoryAsync<Article, int>
    {
        [Statement(Id = "GetEntity")]
        Task<Article> GetByIdAsync([Param("Id")] int id, [Param("IsDraft")] bool isDraft = false);

        Task<int> InsertAsync(object parameters);

        Task<T> QueryByPageAsync<T>(object parameters);

        Task<int> UpdateAsync(object parameters);
    }
}