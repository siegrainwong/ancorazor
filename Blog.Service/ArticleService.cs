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
            ISmartSqlMapper mapper, ITagRepository tagRepository)
        {
            ArticleRepository = articleRepository;
            CategoryRepository = categoryRepository;
            _mapper = mapper;
            TagRepository = tagRepository;
        }
        
        public IArticleRepository ArticleRepository { get; }
        public ITagRepository TagRepository { get; }
        public ICategoryRepository CategoryRepository { get; }

        public async Task<int> InsertAsync(ArticleUpdateParameter parameter)
        {
            try
            {
                _mapper.BeginTransaction();

                var id = await ArticleRepository.InsertAsync(parameter);
                await SetArticleTagsAndCategories(id, parameter.Tags, parameter.Categories);

                _mapper.CommitTransaction();
                return id;
            }
            catch (Exception)
            {
                _mapper.RollbackTransaction();
                throw;
            }
        }

        public async Task<bool> UpdateAsync(ArticleUpdateParameter parameter)
        {
            try
            {
                _mapper.BeginTransaction();

                var externalTask = SetArticleTagsAndCategories(parameter.Id, parameter.Tags, parameter.Categories);
                var updateTask = ArticleRepository.UpdateAsync(parameter);
                await Task.WhenAll(externalTask, updateTask);

                _mapper.CommitTransaction();
                return true;
            }
            catch (Exception)
            {
                _mapper.RollbackTransaction();
                throw;
            }
        }

        private Task<int[]> SetArticleTagsAndCategories(int articleId, string[] tags, string[] categories)
        {
            var categoryTask = CategoryRepository.SetArticleCategoriesAsync(articleId, categories);
            var tagTask = TagRepository.SetArticleTagsAsync(articleId, tags);
            return Task.WhenAll(categoryTask, tagTask);
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