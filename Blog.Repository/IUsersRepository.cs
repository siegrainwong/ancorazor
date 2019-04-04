#region

using Blog.Entity;
using SmartSql.DyRepository;
using System.Threading.Tasks;

#endregion

namespace Blog.Repository
{
    public interface IUsersRepository : IRepositoryAsync<Users, int>
    {
        new Task<Users> GetByIdAsync(int id);
        Task<Users> GetByLoginNameAsync(string loginName);

        Task<int> UpdateAsync(object parameters);
    }
}