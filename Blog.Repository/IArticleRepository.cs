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
        [UseTransaction]
        Task<int> DeleteAsync(int id);
    }
}