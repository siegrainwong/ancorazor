#region

using Blog.API.Common.Constants;
using Blog.API.Messages;
using Blog.API.Messages.Article;
using Blog.Repository;
using Microsoft.Extensions.Options;
using Siegrain.Common;
using SmartSql.AOP;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

#endregion

namespace Blog.Service
{
    public class ArticleService
    {
        public IArticleRepository Repository { get; }
        private readonly SEOConfiguration _seoConfiguration;

        public ArticleService(IArticleRepository articleRepository, IOptions<SEOConfiguration> seoConfiguration)
        {
            Repository = articleRepository;
            _seoConfiguration = seoConfiguration.Value;
        }

        public async Task<PaginationResponse<ArticleViewModel>> QueryByPageAsync(PaginationParameter request)
        {
            var result = await Repository.QueryByPageAsync<PaginationResponse<ArticleViewModel>>(request);
            foreach (var item in result.List) item.Path = GetArticleRoutePath(item);
            result.PageIndex = request.PageIndex;
            result.PageSize = request.PageSize;
            return result;
        }

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

        private string GetArticleRoutePath(ArticleViewModel viewModel)
        {
            var id = viewModel.Id.ToString();
            var date = viewModel.CreatedAt.ToString("yyyy/MM/dd");
            var title = viewModel.TitlePinyin == null ? CHNToPinyin.ConvertToPinYin(viewModel.Title) : viewModel.TitlePinyin;
            var alias = viewModel.Alias == null ? title : viewModel.Alias;
            var category = viewModel.Category == null ? Constants.Article.DefaultCategoryName : CHNToPinyin.ConvertToPinYin(viewModel.Category);
            
            var path = _seoConfiguration.ArticleRouteMapping
                .Replace(nameof(id), id)
                .Replace(nameof(date), date)
                .Replace(nameof(category), category)
                .Replace(nameof(alias), alias)
                .ToLowerInvariant();
            return Regex.Replace(path, @"([^\w\d\/])+(-*)", "-");
        }
    }
}