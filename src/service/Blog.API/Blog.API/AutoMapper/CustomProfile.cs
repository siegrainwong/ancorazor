using Blog.Model;
using Blog.Model.ViewModel;
using AutoMapper;

namespace Blog.API.AutoMapper
{
    public class CustomProfile : Profile
    {
        /// <summary>
        /// 配置构造函数，用来创建关系映射
        /// </summary>
        public CustomProfile()
        {
            CreateMap<BlogArticle, BlogViewModel>();
        }
    }
}
