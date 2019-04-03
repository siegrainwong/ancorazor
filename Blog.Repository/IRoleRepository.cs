#region

using Blog.Entity;
using SmartSql.DyRepository;
using SmartSql.DyRepository.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;

#endregion

namespace Blog.Repository
{
    public interface IRoleRepository : IRepository<Role, int>
    {
        Task<IEnumerable<Role>> GetRolesByUserAsync(int userId);
    }
}