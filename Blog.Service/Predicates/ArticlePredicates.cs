using Blog.EF.Entity;
using LinqKit;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Blog.Service
{
    public partial class ArticleService
    {
        public static Expression<Func<Article, bool>> IsDraft(bool? isDraft)
        {
            var predicate = PredicateBuilder.New<Article>(true);
            if (!isDraft.HasValue) return predicate;
            return predicate.And(x => x.IsDraft == isDraft);
        }
    }
}
