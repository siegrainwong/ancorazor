#region

using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;
using Blog.Entity;
using SmartSql.DyRepository;

#endregion

namespace Blog.Repository
{
    public interface ICategoryRepository : IRepositoryAsync<Category, int>
    {
        Task SetArticleCategoriesAsync(int articleId, string[] categories);
    }
}