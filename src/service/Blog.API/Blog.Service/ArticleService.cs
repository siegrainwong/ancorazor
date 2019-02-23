#region

using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Blog.IRepository;
using Blog.IService;
using Blog.Model;
using Blog.Model.ViewModel;
using Blog.Service.Base;
using System;
using AutoMapper.Configuration;
using Blog.Common.Helpers;
using Blog.Model.Mapping;
using Blog.Model.ParameterModel;
using Blog.Model.Resources;

#endregion

namespace Blog.Service
{	
	/// <summary>
	/// ArticleService
	/// </summary>	
	public class ArticleService : BaseService<Article>, IArticleService
    {
        private IMapper _mapper;
        private IArticleRepository _dal;

        public ArticleService(IArticleRepository dal, IMapper mapper, IPropertyMappingContainer mappingContainer)
        {
            _dal = dal;
            _mapper = mapper;
            baseDal = dal;
            baseDal.SetMapperContainer(mappingContainer);
        }
        public async Task<List<Article>> GetArticles()
        {
            var list = await _dal.Query();
            return list;
        }

        public async Task<Article> GetArticle(int id)
        {
            if (id <= 0) throw new ArgumentException(nameof(id));
            return await _dal.QueryByID(id);
        }

        public async Task<PaginatedList<Article>> GetPagedArticles(ArticleParameters parameters)
        {
            var predicate = PredicateBuilder.True<Article>();
            if (!string.IsNullOrEmpty(parameters.Title))
            {
                var title = parameters.Title.ToLowerInvariant();
                predicate = predicate.And(x => x.Title.ToLowerInvariant() == title);
            }
            
            return await _dal.QueryPage<ArticleViewModel>(predicate, parameters);
        }
    }
}
	