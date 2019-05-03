#region

using Blog.API.Common.Constants;
using Blog.API.Messages;
using Blog.API.Messages.Article;
using Blog.EF.Entity;
using Blog.Repository;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Siegrain.Common;
using SmartSql.AOP;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

#endregion

namespace Blog.Service
{
    public class ArticleService
    {
        public IArticleRepository Repository { get; }
        private readonly SEOConfiguration _seoConfiguration;
        private readonly BlogContext _context;

        public ArticleService(IArticleRepository articleRepository, IOptions<SEOConfiguration> seoConfiguration, BlogContext context)
        {
            Repository = articleRepository;
            _seoConfiguration = seoConfiguration.Value;
            _context = context;
        }

        public async Task<Article> GetByIdAsync(int id, bool? isDraft)
        {
            var predicate = PredicateBuilder.New<Article>(x => x.Id == id);
            if (isDraft.HasValue) predicate.And(x => x.IsDraft == isDraft);
            return await _context.Article.SingleOrDefaultAsync(predicate);
        }

        public async Task<Article> GetByAliasAsync(string alias, bool? isDraft)
        {
            var predicate = PredicateBuilder.New<Article>(x => x.Alias == alias);
            if (isDraft.HasValue) predicate.And(x => x.IsDraft == isDraft);
            return await _context.Article.SingleOrDefaultAsync(predicate);
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
            ReformmatingArticleData(ref parameter);
            var id = await Repository.InsertAsync(parameter);
            await SetArticleTagsAndCategories(id, parameter.Tags, parameter.Categories);
            return id;
        }

        [Transaction]
        public virtual async Task<bool> UpdateAsync(ArticleUpdateParameter parameter)
        {
            ReformmatingArticleData(ref parameter);
            var externalTask = SetArticleTagsAndCategories(parameter.Id, parameter.Tags, parameter.Categories);
            var updateTask = Repository.UpdateAsync(parameter);
            await Task.WhenAll(externalTask, updateTask);
            return true;
        }

        private void ReformmatingArticleData(ref ArticleUpdateParameter parameter)
        {
            var pinyin = CHNToPinyin.ConvertToPinYin(parameter.Alias ?? parameter.Title);
            parameter.Alias = Regex.Replace(pinyin, Constants.Article.RouteReplaceRegex, 
                " ").Trim().Replace(" ", "-").ToLowerInvariant();
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
            var alias = viewModel.Alias;
            var category = viewModel.Category == null ? Constants.Article.DefaultCategoryName : CHNToPinyin.ConvertToPinYin(viewModel.Category);
            
            var path = _seoConfiguration.ArticleRouteMapping
                .Replace(nameof(id), id)
                .Replace(nameof(date), date)
                .Replace(nameof(category), category)
                .Replace(nameof(alias), alias)
                .ToLowerInvariant();
            return path;
        }
    }
}