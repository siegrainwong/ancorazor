using Blog.Model;
using Blog.Model.ViewModel;
using AutoMapper;
using Blog.Model.ParameterModel;

namespace Blog.API.AutoMapper
{
    public class CustomProfile : Profile
    {
        /// <summary>
        /// 配置构造函数，用来创建关系映射
        /// </summary>
        public CustomProfile()
        {
            CreateMap<Article, ArticleViewModel>();
            CreateMap<ArticleParameters, Article>().IgnoreAllPropertiesWithAnInaccessibleSetter();
        }
    }
}
