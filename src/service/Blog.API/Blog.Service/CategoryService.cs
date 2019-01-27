	//----------Category开始----------
    

using Blog.IRepository;
using Blog.IService;
using Blog.Model;
using Blog.Service.Base;

namespace Blog.Service
{	
	/// <summary>
	/// CategoryService
	/// </summary>	
	public class CategoryService : BaseService<Category>, ICategoryService
    {
	
        ICategoryRepository dal;
        public CategoryService(ICategoryRepository dal)
        {
            this.dal = dal;
            base.baseDal = dal;
        }
       
    }
}

	//----------Category结束----------
	