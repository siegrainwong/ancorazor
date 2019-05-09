#region

using AutoMapper;
using Blog.API.Common.Constants;
using Blog.API.Messages;
using Blog.API.Messages.Article;
using Blog.API.Messages.Exceptions;
using Blog.Entity;
using Blog.Service.Interceptors;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Siegrain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

#endregion

namespace Blog.Service
{
    public partial class ArticleService
    {
        private readonly SEOConfiguration _seoConfiguration;
        private readonly BlogContext _context;
        private readonly IMapper _mapper;

        public ArticleService(IOptions<SEOConfiguration> seoConfiguration, BlogContext context, IMapper mapper)
        {
            _seoConfiguration = seoConfiguration.Value;
            _context = context;
            _mapper = mapper;
        }

        public async Task<Article> GetByIdAsync(int id, bool? isDraft)
        {
            return await _context.Article.SingleOrDefaultAsync(IsDraft(isDraft)
                .And(x => x.Id == id));
        }

        private async Task<Article> GetByIdIncludeAsync(int id)
        {
            return await _getArticleIncludedAsync(_context, id);
        }

        public async Task<Article> GetByAliasAsync(string alias, bool? isDraft)
        {
            var entity = await _getArticleByAliasIncludedAsync(_context, alias);
            if (isDraft.HasValue && entity.IsDraft != isDraft) return null;
            return entity;
        }

        public async Task<PaginationResponse<ArticleViewModel>> QueryByPageAsync(ArticlePaginationParameter parameters)
        {
            var predicate = PredicateBuilder.New<Article>(true);
            if (parameters.IsDraft.HasValue) predicate.And(x => x.IsDraft == parameters.IsDraft);
            var result = new PaginationResponse<ArticleViewModel> { PageIndex = parameters.PageIndex, PageSize = parameters.PageSize };

            // TODO: 这里用扩展方法封装一下
            var futureTotal = _context.Article.DeferredCount(predicate).FutureValue();
            var futureList = _context.Article.Where(predicate)
                .Select(x => new { x.Alias, x.CommentCount, x.Title, x.CreatedAt, x.Id, x.IsDraft, x.Digest, x.ViewCount })
                .Take(parameters.PageSize)
                .Skip(parameters.PageSize * parameters.PageIndex)
                .OrderByDescending(x => x.CreatedAt)
                .Future();

            result.Total = await futureTotal.ValueAsync();
            result.List = _mapper.Map<IEnumerable<ArticleViewModel>>(await futureList.ToListAsync());
            result.PageIndex = parameters.PageIndex;
            result.PageSize = parameters.PageSize;

            foreach (var i in result.List) i.Path = GetArticleRoutePath(i);

            return result;
        }

        [Transaction]
        public virtual async Task<ArticleViewModel> UpsertAsync(ArticleUpdateParameter parameter)
        {
            PreprocessArticleData(parameter);

            Article entity = null;
            if (parameter.Id != 0)
            {
                entity = await GetByIdIncludeAsync(parameter.Id);
                DropRelations(entity);
                entity.UpdatedAt = DateTime.Now;
            }
            _mapper.Map(parameter, entity);

            var categories = await _context.Category
                .Where(x => parameter.Categories.Contains(x.Name)).ToListAsync();
            var tags = await _context.Tag
                .Where(x => parameter.Tags.Contains(x.Name)).ToListAsync();

            var newCategories = parameter.Categories
                .Except(categories.Select(x => x.Name))
                .Select(x => new Category { Name = x });
            var newTags = parameter.Tags
                .Except(tags.Select(x => x.Name))
                .Select(x => new Tag { Name = x });

            await _context.ArticleCategories
                .AddRangeAsync(categories.Concat(newCategories)
                .Select(x => new ArticleCategories
                {
                    ArticleNavigation = entity,
                    CategoryNavigation = x
                }));
            await _context.ArticleTags
                .AddRangeAsync(tags.Concat(newTags)
                .Select(x => new ArticleTags
                {
                    ArticleNavigation = entity,
                    TagNavigation = x
                }));

            await _context.SaveChangesAsync();
            await DropUnusedTagsAndCategories();

            var viewModel = _mapper.Map<ArticleViewModel>(entity);
            viewModel.Path = GetArticleRoutePath(viewModel);
            return viewModel;
        }

        [Transaction]
        public virtual async Task<bool> DeleteAsync(int id)
        {
            var entity = await GetByIdIncludeAsync(id);

            DropRelations(entity);
            _context.Article.Remove(entity);

            // ensure relational data has been removed
            // otherwise redundancies won't remove from stmts below.
            await _context.SaveChangesAsync();

            await DropUnusedTagsAndCategories();
            return true;
        }

        private void DropRelations(Article entity)
        {
            // drop relational data of the article
            _context.ArticleCategories.RemoveRange(entity.ArticleCategories);
            _context.ArticleTags.RemoveRange(entity.ArticleTags);
        }

        private async Task<bool> DropUnusedTagsAndCategories()
        {
            _context.Tag.RemoveRange(await _getUnusedTags(_context).ToListAsync());
            _context.Category.RemoveRange(await _getUnusedCategories(_context).ToListAsync());
            await _context.SaveChangesAsync();
            return true;
        }

        private void PreprocessArticleData(ArticleUpdateParameter parameter)
        {
            var pinyin = CHNToPinyin.ConvertToPinYin(parameter.Alias ?? parameter.Title);
            parameter.Alias = Regex.Replace(pinyin, Constants.Article.RouteReplaceRegex,
                " ").Trim().Replace(" ", "-").ToLowerInvariant();

            if (parameter.Id == 0 &&
                _context.Article.Any(x => x.Title == parameter.Title || x.Alias == parameter.Alias))
                throw new DuplicateEntityException("Duplicate title or alias of an article.");
        }

        private string GetArticleRoutePath(ArticleViewModel viewModel)
        {
            var id = viewModel.Id.ToString();
            var date = viewModel.CreatedAt.ToString("yyyy/MM/dd");
            var alias = viewModel.Alias;
            var category = viewModel.ArticleCategories.FirstOrDefault()?.CategoryNavigation.Name ?? Constants.Article.DefaultCategoryName;

            var path = _seoConfiguration.ArticleRouteMapping
                .Replace(nameof(id), id)
                .Replace(nameof(date), date)
                .Replace(nameof(category), CHNToPinyin.ConvertToPinYin(category))
                .Replace(nameof(alias), alias)
                .ToLowerInvariant();
            return path;
        }
    }
}