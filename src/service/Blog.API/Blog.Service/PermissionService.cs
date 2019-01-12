	//----------Permission开始----------
    

using Blog.IRepository;
using Blog.IService;
using Blog.Model;
using Blog.Service.Base;

namespace Blog.Service
{	
	/// <summary>
	/// PermissionService
	/// </summary>	
	public class PermissionService : BaseService<Permission>, IPermissionService
    {
	
        IPermissionRepository dal;
        public PermissionService(IPermissionRepository dal)
        {
            this.dal = dal;
            base.baseDal = dal;
        }
       
    }
}

	//----------Permission结束----------
	