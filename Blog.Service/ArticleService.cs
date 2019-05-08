#region

using AutoMapper;
using Blog.API.Common.Constants;
using Blog.API.Messages;
using Blog.API.Messages.Article;
using Blog.API.Messages.Exceptions;
using Blog.EF.Entity;
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

        public async Task<Article> GetByAliasAsync(string alias, bool? isDraft)
        {
            return await _context.Article.SingleOrDefaultAsync(IsDraft(isDraft)
                .And(x => x.Alias == alias));
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
            // TODO: AOP transaction
            var transaction = _context.Database.BeginTransaction();
            try
            {
                PreprocessArticleData(parameter);

                // 这个删除操作必须放在第一个上下文操作中，不然ef会跳线程，不知道里面怎么实现的。
                var entity = _mapper.Map<Article>(parameter);
                if (entity.Id != 0)
                {
                    await ResetArticleRelationalData(entity.Id);
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
            catch (Exception e)
            {
                transaction.Rollback();
                throw e;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            await ResetArticleRelationalData(id);
            await _context.Article.Where(x => x.Id == id).DeleteAsync();

            await _context.BulkSaveChangesAsync();
            return true;
        }

        // TODO: transaction
        private async Task<bool> ResetArticleRelationalData(int articleId)
        {
            // drop middle-table data of the article
            await _context.ArticleCategories.Where(x => x.Article == articleId).DeleteAsync();
            await _context.ArticleTags.Where(x => x.Article == articleId).DeleteAsync();
            // drop unused tags and categories
            await _context.Tag.Where(x => !_context.ArticleTags.Any(y => y.Tag == x.Id)).DeleteAsync();
            await _context.Category.Where(x => !_context.ArticleCategories.Any(y => y.Category == y.Id)).DeleteAsync();
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