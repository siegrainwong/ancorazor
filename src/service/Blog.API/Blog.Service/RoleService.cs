	//----------Role开始----------
    

using Blog.IRepository;
using Blog.IService;
using Blog.Model;
using Blog.Service.Base;

namespace Blog.Service
{	
	/// <summary>
	/// RoleService
	/// </summary>	
	public class RoleService : BaseService<Role>, IRoleService
    {
	
        IRoleRepository dal;
        public RoleService(IRoleRepository dal)
        {
            this.dal = dal;
            base.baseDal = dal;
        }
       
    }
}

	//----------Role结束----------
	