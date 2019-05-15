#region

using AutoMapper;
using Blog.API.Common;
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
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

#endregion

namespace Blog.Service
{
    public partial class ArticleService
    {
        private readonly UrlHelper _urlHelper;
        private readonly BlogContext _context;
        private readonly IMapper _mapper;
        private readonly SiteSettingService _settingService;

        public ArticleService(UrlHelper urlHelper, BlogContext context, IMapper mapper, SiteSettingService settingService)
        {
            _urlHelper = urlHelper;
            _context = context;
            _mapper = mapper;
            _settingService = settingService;
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

        public async Task<object> GetByAliasAsync(string alias, bool? isDraft)
        {
            var viewModel = await GetArticleByAliasAsync(alias, isDraft);

            if (viewModel.Previous != null)
                viewModel.Previous.Path = GetArticleRoutePath(viewModel.Previous);
            if (viewModel.Next != null)
                viewModel.Next.Path = GetArticleRoutePath(viewModel.Next);
            return viewModel;
        }

        public async Task<PaginationResponse<ArticleViewModel>> QueryByPageAsync(ArticlePaginationParameter parameters)
        {
            var predicate = PredicateBuilder.New<Article>(true);
            if (parameters.IsDraft.HasValue) predicate.And(x => x.IsDraft == parameters.IsDraft);
            var result = new PaginationResponse<ArticleViewModel> { PageIndex = parameters.PageIndex, PageSize = parameters.PageSize };

            // TODO: 这里用扩展方法封装一下
            var futureTotal = _context.Article.DeferredCount(predicate).FutureValue();
            var futureList = _context.Article.Where(predicate)
                .Select(x => new { x.Alias, x.CommentCount, x.Title, x.CreatedAt, x.Id, x.IsDraft, x.Digest, x.ViewCount, CategoryAlias = x.CategoryNavigation.Alias })
                .OrderByDescending(x => x.CreatedAt)
                .Skip(parameters.PageSize * parameters.PageIndex)
                .Take(parameters.PageSize)
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
            parameter.Alias = UrlHelper.ToUrlSafeString(parameter.Alias ?? parameter.Title, true);
            if (parameter.Id == 0 &&
                _context.Article.Any(x => x.Title == parameter.Title || x.Alias == parameter.Alias))
                throw new DuplicateEntityException<Article>(nameof(Article.Title), nameof(Article.Alias));

            var isUpdate = parameter.Id != 0;
            Article entity = null;
            if (isUpdate)
            {
                entity = await GetByIdIncludeAsync(parameter.Id);
                if (entity == null) throw new EntityNotFoundException<Article>();
                DropRelations(entity);
                entity.UpdatedAt = DateTime.Now;
                _mapper.Map(parameter, entity);
            }
            else
            {
                entity = _mapper.Map<Article>(parameter);
            }

            var category = await _context.Category
                .FirstOrDefaultAsync(x => parameter.Category == x.Name);
            if (category == null)
            {
                entity.CategoryNavigation = new Category
                {
                    Name = parameter.Category,
                    Alias = UrlHelper.UrlStringEncode(parameter.Category),
                };
            }

            var tags = await _context.Tag
                .Where(x => parameter.Tags.Contains(x.Name)).ToListAsync();

            var newTags = parameter.Tags
                .Except(tags.Select(x => x.Name))
                .Select(x => new Tag { Name = x, Alias = UrlHelper.UrlStringEncode(x) });

            if (!isUpdate) await _context.Article.AddAsync(entity);

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
            if (entity == null) throw new EntityNotFoundException<Article>();

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
            _context.ArticleTags.RemoveRange(entity.ArticleTags);
        }

        private async Task<bool> DropUnusedTagsAndCategories()
        {
            _context.Tag.RemoveRange(await _getUnusedTags(_context).ToListAsync());
            _context.Category.RemoveRange(await _getUnusedCategories(_context).ToListAsync());
            await _context.SaveChangesAsync();
            return true;
        }

        private string GetArticleRoutePath(ArticleViewModel viewModel)
        {
            if (viewModel == null) return null;
            var setting = _settingService.GetSetting();
            return _urlHelper.GetArticleRoutePath(viewModel.Id, viewModel.CreatedAt, viewModel.Alias, viewModel.CategoryAlias, setting.RouteMapping);
        }
    }
}