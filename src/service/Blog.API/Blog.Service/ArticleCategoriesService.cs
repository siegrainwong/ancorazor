	//----------ArticleCategories开始----------
    

using Blog.IRepository;
using Blog.IService;
using Blog.Model;
using Blog.Service.Base;

namespace Blog.Service
{	
	/// <summary>
	/// ArticleCategoriesService
	/// </summary>	
	public class ArticleCategoriesService : BaseService<ArticleCategories>, IArticleCategoriesService
    {
	
        IArticleCategoriesRepository dal;
        public ArticleCategoriesService(IArticleCategoriesRepository dal)
        {
            this.dal = dal;
            base.baseDal = dal;
        }
       
    }
}

	//----------ArticleCategories结束----------
	