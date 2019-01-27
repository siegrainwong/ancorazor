	//----------Users开始----------
    

using Blog.IRepository;
using Blog.IService;
using Blog.Model;
using Blog.Service.Base;

namespace Blog.Service
{	
	/// <summary>
	/// UsersService
	/// </summary>	
	public class UsersService : BaseService<Users>, IUsersService
    {
	
        IUsersRepository dal;
        public UsersService(IUsersRepository dal)
        {
            this.dal = dal;
            base.baseDal = dal;
        }
       
    }
}

	//----------Users结束----------
	