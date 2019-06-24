using AutoMapper;
using Ancorazor.API.Messages;
using Ancorazor.API.Messages.Article;
using Ancorazor.Entity;

namespace Ancorazor.API.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Article, ArticleViewModel>();
            CreateMap<ArticleViewModel, Article>();
            CreateMap<ArticleUpdateParameter, Article>()
                .ForMember(x => x.Category, o => o.Ignore())
                .ForAllMembers(o => o.Condition(
                    (source, destination, sourceMember, destMember) => 
                    (sourceMember != null)));
        }
    }
}
