	//----------UserRole开始----------
    

using Blog.IRepository;
using Blog.IService;
using Blog.Model;
using Blog.Service.Base;

namespace Blog.Service
{	
	/// <summary>
	/// UserRoleService
	/// </summary>	
	public class UserRoleService : BaseService<UserRole>, IUserRoleService
    {
	
        IUserRoleRepository dal;
        public UserRoleService(IUserRoleRepository dal)
        {
            this.dal = dal;
            base.baseDal = dal;
        }
       
    }
}

	//----------UserRole结束----------
	