#region

using System.Threading.Tasks;
using Blog.Entity;
using SmartSql.DyRepository;

#endregion

namespace Blog.Repository
{
    
    public interface IArticleRepository : IRepositoryAsync<Article, int>
    {
        [Statement(Id = "QueryByPage")]
        Task<T> QueryByPageAsync<T>(object parameters);

        [Statement(Id = "Insert")]
        Task<int> InsertAsync(object parameters);

        [Statement(Id = "Update")]
        Task<int> UpdateAsync(object parameters);

        [Statement(Id = "GetEntity")]
        Task<Article> GetByIdAsync([Param("Id")] int id, [Param("IsDeleted")] bool isDeleted = false);
    }
}