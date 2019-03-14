#region

using Blog.Entity;
using Blog.Repository;

#endregion

namespace Blog.Service
{
    public class RoleService
    {
        public RoleService(IRoleRepository roleRepository)
        {
            RoleRepository = roleRepository;
        }

        public IRoleRepository RoleRepository { get; }

        public int Insert(Role role)
        {
            return RoleRepository.Insert(role);
        }

        public int DeleteById(int id)
        {
            return RoleRepository.DeleteById(id);
        }

        public int Update(Role role)
        {
            return RoleRepository.Update(role);
        }
    }
}