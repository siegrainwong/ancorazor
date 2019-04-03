#region

using Blog.API.Messages;
using Blog.API.Messages.Article;
using Blog.Entity;
using Blog.Repository;
using SmartSql.Abstractions;
using System;
using System.Threading.Tasks;

#endregion

namespace Blog.Service
{
    public class ArticleService
    {
        private readonly ISmartSqlMapper _mapper;

        public IArticleRepository ArticleRepository { get; }

        public ICategoryRepository CategoryRepository { get; }

        public ITagRepository TagRepository { get; }

        public ArticleService(IArticleRepository articleRepository, ICategoryRepository categoryRepository,
                                    ISmartSqlMapper mapper, ITagRepository tagRepository)
        {
            ArticleRepository = articleRepository;
            CategoryRepository = categoryRepository;
            _mapper = mapper;
            TagRepository = tagRepository;
        }

        public async Task<int> InsertAsync(ArticleUpdateParameter parameter)
        {
            try
            {
                _mapper.BeginTransaction();

                int id = await ArticleRepository.InsertAsync(parameter);
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

        public async Task<QueryByPageResponse<Article>> QueryByPageAsync(QueryByPageParameter request)
        {
            QueryByPageResponse<Article> result = await ArticleRepository.QueryByPageAsync<QueryByPageResponse<Article>>(request);
            result.PageIndex = request.PageIndex;
            result.PageSize = request.PageSize;
            return result;
        }

        public async Task<bool> UpdateAsync(ArticleUpdateParameter parameter)
        {
            try
            {
                _mapper.BeginTransaction();

                Task externalTask = SetArticleTagsAndCategories(parameter.Id, parameter.Tags, parameter.Categories);
                Task<int> updateTask = ArticleRepository.UpdateAsync(parameter);
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

        private Task SetArticleTagsAndCategories(int articleId, string[] tags, string[] categories)
        {
            Task tagTask = TagRepository.SetArticleTagsAsync(articleId, tags);
            Task categoryTask = CategoryRepository.SetArticleCategoriesAsync(articleId, categories);
            return Task.WhenAll(
             tagTask,
             categoryTask
            );
        }
    }
}