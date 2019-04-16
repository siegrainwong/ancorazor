#region

using Blog.Entity;
using SmartSql.DyRepository;
using System.Threading.Tasks;

#endregion

namespace Blog.Repository
{
    public interface ICategoryRepository : IRepositoryAsync<Category, int>
    {
    }
}