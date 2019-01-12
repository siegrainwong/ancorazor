	//----------RoleModulePermission开始----------
    

using Blog.IRepository;
using Blog.IService;
using Blog.Model;
using Blog.Service.Base;

namespace Blog.Service
{	
	/// <summary>
	/// RoleModulePermissionService
	/// </summary>	
	public class RoleModulePermissionService : BaseService<RoleModulePermission>, IRoleModulePermissionService
    {
	
        IRoleModulePermissionRepository dal;
        public RoleModulePermissionService(IRoleModulePermissionRepository dal)
        {
            this.dal = dal;
            base.baseDal = dal;
        }
       
    }
}

	//----------RoleModulePermission结束----------
	