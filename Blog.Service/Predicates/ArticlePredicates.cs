using Blog.Entity;
using LinqKit;
using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

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
    }
}
