#region

using System.Threading.Tasks;
using Blog.Entity;
using SmartSql.DyRepository;

#endregion

namespace Blog.Repository
{
    
    public interface IArticleRepository : IRepositoryAsync<Article, int>
    {
        Task<T> QueryByPageAsync<T>(object parameters);
        
        Task<int> InsertAsync(object parameters);
        
        Task<int> UpdateAsync(object parameters);

        [Statement(Id = "GetEntity")]
        Task<Article> GetByIdAsync([Param("Id")] int id, [Param("IsDeleted")] bool isDeleted = false);
    }
}