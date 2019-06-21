using Ancorazor.Entity;
using LinqKit;
using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Query;
using Z.EntityFramework.Plus;
using Ancorazor.API.Messages.Article;
using Ancorazor.API.Messages.Exceptions;

namespace Ancorazor.Service
{
    public partial class ArticleService
    {
        private static Expression<Func<Article, bool>> IsDraft(bool? isDraft)
        {
            var predicate = PredicateBuilder.New<Article>(true);
            if (!isDraft.HasValue) return predicate;
            return predicate.And(x => x.IsDraft == isDraft);
        }

        /*
         * MARK: ef core compiled queries
         * https://gunnarpeipman.com/data/ef-core-compiled-queries/
         * 
         * MARK: ef core `include` execute separately
         * https://stackoverflow.com/a/34732579
         */

        private static readonly Func<BlogContext, int, Task<Article>> _getArticleIncludedAsync =
            EF.CompileAsyncQuery((BlogContext context, int id) =>
                context.Article
                    .Include(x => x.CategoryNavigation)
                    .Include(x => x.ArticleTags)
                    .FirstOrDefault(x => x.Id == id));

        private async Task<ArticleViewModel> GetArticleByAliasAsync(string alias, bool? isDraft)
        {
            var futureArticle = _context.Article
                    .Select(x => new
                    {
                        x.Alias,
                        x.Content,
                        x.Title,
                        x.Id,
                        x.CommentCount,
                        x.ViewCount,
                        x.IsDraft,
                        x.CreatedAt,
                        Cover = x.ImageStorageNavigation.Path,
                        Author = x.AuthorNavigation.RealName,
                        Tags = x.ArticleTags.Select(c => new
                        {
                            c.TagNavigation.Name,
                            c.TagNavigation.Alias
                        }),
                        Category = x.CategoryNavigation
                    })
                    .DeferredFirstOrDefault(x => x.Alias == alias).FutureValue();

            var article = _mapper.Map<ArticleViewModel>(await futureArticle.ValueAsync());

            if (isDraft.HasValue && article.IsDraft != isDraft)
                throw new EntityNotFoundException<Article>();

            var futurePrevious = _context.Article.Where(p => p.CreatedAt > article.CreatedAt)
                            .OrderBy(p => p.CreatedAt).Select(p => new
                            {
                                p.Id,
                                p.CreatedAt,
                                p.Title,
                                p.Alias,
                                Path = "",
                                Category = p.CategoryNavigation
                            }).DeferredFirstOrDefault().FutureValue();

            var futureNext = _context.Article.Where(p => p.CreatedAt < article.CreatedAt)
                            .OrderByDescending(p => p.CreatedAt).Select(p => new
                            {
                                p.Id,
                                p.CreatedAt,
                                p.Title,
                                p.Alias,
                                Path = "",
                                Category = p.CategoryNavigation
                            }).DeferredFirstOrDefault().FutureValue();

            article.Previous = _mapper.Map<ArticleViewModel>(await futurePrevious.ValueAsync());
            article.Next = _mapper.Map<ArticleViewModel>(await futureNext.ValueAsync());

            return article;
        }

        private static readonly Func<BlogContext, AsyncEnumerable<Tag>> _getUnusedTags =
            EF.CompileAsyncQuery((BlogContext context) =>
                context.Tag.Where(x => !context.ArticleTags.Select(y => y.Tag).Contains(x.Id)));

        private static readonly Func<BlogContext, AsyncEnumerable<Category>> _getUnusedCategories =
            EF.CompileAsyncQuery((BlogContext context) =>
                context.Category.Where(x => !x.Article.Select(y => y.Category).Contains(x.Id)));
    }
}
