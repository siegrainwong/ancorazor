using Blog.Entity;
using LinqKit;
using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Query;

namespace Blog.Service
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
         */

        private static readonly Func<BlogContext, int, Task<Article>> _getArticleIncludedAsync =
            EF.CompileAsyncQuery((BlogContext context, int id) =>
                context.Article
                    .Include(x => x.ArticleCategories)
                    .Include(x => x.ArticleTags)
                    .FirstOrDefault(x => x.Id == id));

        private static readonly Func<BlogContext, string, Task<Article>> _getArticleByAliasIncludedAsync =
            EF.CompileAsyncQuery((BlogContext context, string alias) =>
                context.Article
                    .Include(x => x.ArticleCategories)
                    .Include(x => x.ArticleTags)
                    .FirstOrDefault(x => x.Alias == alias));

        private static readonly Func<BlogContext, AsyncEnumerable<Tag>> _getUnusedTags =
            EF.CompileAsyncQuery((BlogContext context) =>
                context.Tag.Where(x => !context.ArticleTags.Select(y => y.Tag).Contains(x.Id)));

        private static readonly Func<BlogContext, AsyncEnumerable<Category>> _getUnusedCategories =
            EF.CompileAsyncQuery((BlogContext context) =>
                context.Category.Where(x => !context.ArticleCategories.Select(y => y.Category).Contains(x.Id)));
    }
}
