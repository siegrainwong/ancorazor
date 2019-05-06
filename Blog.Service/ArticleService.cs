#region

using AutoMapper;
using Blog.API.Common.Constants;
using Blog.API.Messages;
using Blog.API.Messages.Article;
using Blog.API.Messages.Exceptions;
using Blog.EF.Entity;
using Blog.Repository;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Siegrain.Common;
using SmartSql.AOP;
using System;
using System.Collections.Generic;
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
        private readonly IMapper _mapper;

        public ArticleService(IArticleRepository articleRepository, IOptions<SEOConfiguration> seoConfiguration, BlogContext context, IMapper mapper)
        {
            Repository = articleRepository;
            _seoConfiguration = seoConfiguration.Value;
            _context = context;
            _mapper = mapper;
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

        public async Task<ArticleViewModel> UpsertAsync(ArticleUpdateParameter parameter)
        {
            var transaction = _context.Database.BeginTransaction();
            try
            {
                PreprocessArticleData(parameter);

                var entity = _mapper.Map<Article>(parameter);
                if (entity.Id != 0)
                {
                    await _context.ArticleCategories.Where(x => x.Article == entity.Id).DeleteAsync();
                    await _context.ArticleTags.Where(x => x.Article == entity.Id).DeleteAsync();
                    entity.UpdatedAt = DateTime.Now;
                }

                var categories = await _context.Category.Where(x => parameter.Categories.Contains(x.Name)).ToListAsync();
                var tags = await _context.Tag.Where(x => parameter.Tags.Contains(x.Name)).ToListAsync();

                var newCategories = parameter.Categories.Except(categories.Select(x => x.Name))
                    .Select(x => new Category { Name = x });
                var newTags = parameter.Tags.Except(tags.Select(x => x.Name))
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

                await _context.BulkSaveChangesAsync();
                transaction.Commit();

                var viewModel = _mapper.Map<ArticleViewModel>(entity);
                viewModel.Path = GetArticleRoutePath(viewModel);

                return viewModel;
            }
            catch(Exception e)
            {
                transaction.Rollback();
                throw e;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            //TODO:
            await _context.ArticleCategories.Where(x => x.Article == id).DeleteAsync();
            await _context.ArticleTags.Where(x => x.Article == id).DeleteAsync();
            await _context.Article.Where(x => x.Id == id).DeleteAsync();
            await _context.Tag.Where(x => !_context.ArticleTags.Any(y=>y.Tag == id)).DeleteAsync();

            await _context.BulkSaveChangesAsync();
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