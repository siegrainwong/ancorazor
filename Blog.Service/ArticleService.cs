#region

using Blog.API.Messages;
using Blog.API.Messages.Article;
using Blog.Entity;
using Blog.Repository;
using System;
using System.Threading.Tasks;
//using SmartSql.AOP;

#endregion

namespace Blog.Service
{
    public class ArticleService
    {
        public IArticleRepository ArticleRepository { get; }

        public ICategoryRepository CategoryRepository { get; }

        public ITagRepository TagRepository { get; }

        public ArticleService(IArticleRepository articleRepository, ICategoryRepository categoryRepository, ITagRepository tagRepository)
        {
            ArticleRepository = articleRepository;
            CategoryRepository = categoryRepository;
            TagRepository = tagRepository;
        }

        //[Transaction]
        public async Task<int> InsertAsync(ArticleUpdateParameter parameter)
        {
            try
            {
                ArticleRepository.SqlMapper.BeginTransaction();

                int id = await ArticleRepository.InsertAsync(parameter);
                await SetArticleTagsAndCategories(id, parameter.Tags, parameter.Categories);

                ArticleRepository.SqlMapper.CommitTransaction();
                return id;
            }
            catch (Exception)
            {
                ArticleRepository.SqlMapper.RollbackTransaction();
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
                ArticleRepository.SqlMapper.BeginTransaction();

                Task externalTask = SetArticleTagsAndCategories(parameter.Id, parameter.Tags, parameter.Categories);
                Task<int> updateTask = ArticleRepository.UpdateAsync(parameter);
                await Task.WhenAll(externalTask, updateTask);

                ArticleRepository.SqlMapper.CommitTransaction();
                return true;
            }
            catch (Exception)
            {
                ArticleRepository.SqlMapper.RollbackTransaction();
                throw;
            }
        }

        private Task SetArticleTagsAndCategories(int articleId, string[] tags, string[] categories)
        {
            Task tagTask = TagRepository.SetArticleTagsAsync(articleId, tags);
            Task categoryTask = CategoryRepository.SetArticleCategoriesAsync(articleId, categories);
            return Task.WhenAll(tagTask, categoryTask);
        }
    }
}