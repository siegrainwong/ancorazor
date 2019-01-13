//----------BlogArticle开始----------


using System.Collections.Generic;
using System.Threading.Tasks;
using Blog.Common.Attributes;
using Blog.IRepository;
using Blog.IService;
using Blog.Model;
using Blog.Service.Base;

namespace Blog.Service
{
    /// <summary>
    /// BlogArticleService
    /// </summary>	
    public class BlogArticleService : BaseService<BlogArticle>, IBlogArticleService
    {
        IBlogArticleRepository dal;
        public BlogArticleService(IBlogArticleRepository dal)
        {
            this.dal = dal;
            base.baseDal = dal;
        }

        public async Task<List<BlogArticle>> GetBlogs()
        {
            var bloglist = await dal.Query(a => a.Id > 0);
            return bloglist;
        }
    }
}

//----------BlogArticle结束----------
