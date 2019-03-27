#region

using System;
using System.Threading.Tasks;
using Blog.API.Messages;
using Blog.API.Messages.Article;
using Blog.Entity;
using Blog.Repository;
using SmartSql.Abstractions;

#endregion

namespace Blog.Service
{
    public class ArticleService
    {
        private ISmartSqlMapper _mapper;

        public ArticleService(IArticleRepository articleRepository, ICategoryRepository categoryRepository,
            ISmartSqlMapper mapper)
        {
            ArticleRepository = articleRepository;
            CategoryRepository = categoryRepository;
            _mapper = mapper;
        }
        
        public IArticleRepository ArticleRepository { get; }
        public ICategoryRepository CategoryRepository { get; }

        public async Task<int> Insert(Article article)
        {
            return await ArticleRepository.InsertAsync(article);
        }

        public async Task<bool> UpdateAsync(ArticleUpdateParameter parameter)
        {
            try
            {
                _mapper.BeginTransaction();

                await CategoryRepository.SetArticleCategoriesAsync(parameter.Id, parameter.Categories);
                var result = await ArticleRepository.UpdateAsync(parameter);

                _mapper.CommitTransaction();
                return result > 0;
            }
            catch (Exception)
            {
                _mapper.RollbackTransaction();
                throw;
            }
        }

        public async Task<QueryByPageResponse<Article>> QueryByPageAsync(QueryByPageParameter request)
        {
            var result = await ArticleRepository.QueryByPageAsync<QueryByPageResponse<Article>>(request);
            result.PageIndex = request.PageIndex;
            result.PageSize = request.PageSize;
            return result;
        }
    }
}