	//----------SysUser开始----------
    

using Blog.IRepository;
using Blog.IService;
using Blog.Model;
using Blog.Service.Base;

namespace Blog.Service
{	
	/// <summary>
	/// SysUserService
	/// </summary>	
	public class SysUserService : BaseService<SysUser>, ISysUserService
    {
	
        ISysUserRepository dal;
        public SysUserService(ISysUserRepository dal)
        {
            this.dal = dal;
            base.baseDal = dal;
        }
       
    }
}

	//----------SysUser结束----------
	