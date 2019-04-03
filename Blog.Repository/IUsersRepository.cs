#region

using Blog.Entity;
using SmartSql.DyRepository;

#endregion

namespace Blog.Repository
{
    public interface IUsersRepository : IRepositoryAsync<Users, int>
    {
    }
}