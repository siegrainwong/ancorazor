#region

using Blog.Entity;
using SmartSql.DyRepository;
using System.Collections.Generic;
using System.Threading.Tasks;

#endregion

namespace Blog.Repository
{
    public interface IRoleRepository : IRepository<Role, int>
    {
        [Statement(Id = "GetRolesByUser")]
        Task<IEnumerable<Role>> GetRolesByUserAsync(int userId);
    }
}