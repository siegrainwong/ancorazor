#region

using Blog.API.Messages;
using Blog.API.Messages.Article;
using Blog.Entity;
using Blog.Repository;
using SmartSql.AOP;
using System.Threading.Tasks;

#endregion

namespace Blog.Service
{
    public class ArticleService
    {
        public IArticleRepository Repository { get; }

        public ArticleService(IArticleRepository articleRepository)
        {
            Repository = articleRepository;
        }

        public async Task<PaginationResponse<Article>> QueryByPageAsync(PaginationParameter request)
        {
            var result = await Repository.QueryByPageAsync<PaginationResponse<Article>>(request);
            result.PageIndex = request.PageIndex;
            result.PageSize = request.PageSize;
            return result;
        }

        //[Transaction]
        //public virtual async Task<bool> DeleteAsync(int id)
        //    => await Repository.DeleteAsync(id) > 0;

        [Transaction]
        public virtual async Task<int> InsertAsync(ArticleUpdateParameter parameter)
        {
            var id = await Repository.InsertAsync(parameter);
            await SetArticleTagsAndCategories(id, parameter.Tags, parameter.Categories);
            return id;
        }

        [Transaction]
        public virtual async Task<bool> UpdateAsync(ArticleUpdateParameter parameter)
        {
            var externalTask = SetArticleTagsAndCategories(parameter.Id, parameter.Tags, parameter.Categories);
            var updateTask = Repository.UpdateAsync(parameter);
            await Task.WhenAll(externalTask, updateTask);
            return true;
        }

        private Task SetArticleTagsAndCategories(int articleId, string[] tags, string[] categories)
        {
            // 这里要在 ConnectionString 中打开 MultipleActiveResultSets，允许一个连接返回多个结果集。
            var t1 = Repository.SetArticleTagsAsync(articleId, tags);
            var t2 = Repository.SetArticleCategoriesAsync(articleId, categories);
            return Task.WhenAll(t1, t2);
        }
    }
}