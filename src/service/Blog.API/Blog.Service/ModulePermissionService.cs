	//----------ModulePermission开始----------
    

using Blog.IRepository;
using Blog.IService;
using Blog.Model;
using Blog.Service.Base;

namespace Blog.Service
{	
	/// <summary>
	/// ModulePermissionService
	/// </summary>	
	public class ModulePermissionService : BaseService<ModulePermission>, IModulePermissionService
    {
	
        IModulePermissionRepository dal;
        public ModulePermissionService(IModulePermissionRepository dal)
        {
            this.dal = dal;
            base.baseDal = dal;
        }
       
    }
}

	//----------ModulePermission结束----------
	