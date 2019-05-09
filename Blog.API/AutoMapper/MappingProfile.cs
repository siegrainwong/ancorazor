using AutoMapper;
using Blog.API.Messages;
using Blog.API.Messages.Article;
using Blog.Entity;

namespace Blog.API.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Article, ArticleViewModel>();
            CreateMap<ArticleViewModel, Article>();
        }
    }
}
