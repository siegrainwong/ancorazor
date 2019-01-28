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
using Blog.Common.Helpers;
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

        public ArticleService(IArticleRepository dal
            //IMapper mapper
            )
        {
            _dal = dal;
            //_mapper = mapper;
            baseDal = dal;
        }
        public async Task<List<Article>> GetArticles()
        {
            var list = await _dal.Query();
            return list;
        }

        public async Task<PaginatedList<Article>> GetPagedArticles(ArticleParameters parameters)
        {
            var predicate = PredicateBuilder.True<Article>();
            if (!string.IsNullOrEmpty(parameters.Title))
            {
                var title = parameters.Title.ToLowerInvariant();
                predicate = predicate.And(x => x.Title.ToLowerInvariant() == title);
            }
            
            return await _dal.QueryPage(predicate, parameters);
        }

        public async Task<ArticleViewModel> GetArticle(int id)
        {
            var list = await GetArticles();
            var article = (await _dal.Query(a => a.Id == id)).FirstOrDefault();
            if (article == null) return new ArticleViewModel();

            var models = _mapper.Map<ArticleViewModel>(article);

            var index = list.FindIndex(item => item.Id == id);
            if (index >= 0)
            {
                // 上一篇
                var prev = index > 0 ? list[index - 1] : null;
                if (prev != null)
                {
                    models.Previous = prev.Title;
                    models.PreviousId = prev.Id;
                }
                // 下一篇
                var next = index + 1 < list.Count ? list[index + 1] : null;
                if (next != null)
                {
                    models.Next = next.Title;
                    models.NextId = next.Id;
                }
            }
            article.ViewCount += 1;
            await _dal.Update(article, new List<string> { nameof(article.ViewCount) });

            return models;
        }
    }
}
	