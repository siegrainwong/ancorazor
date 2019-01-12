	//----------Module开始----------
    

using Blog.IRepository;
using Blog.IService;
using Blog.Model;
using Blog.Service.Base;

namespace Blog.Service
{	
	/// <summary>
	/// ModuleService
	/// </summary>	
	public class ModuleService : BaseService<Module>, IModuleService
    {
	
        IModuleRepository dal;
        public ModuleService(IModuleRepository dal)
        {
            this.dal = dal;
            base.baseDal = dal;
        }
       
    }
}

	//----------Module结束----------
	