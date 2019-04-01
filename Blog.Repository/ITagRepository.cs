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
    public interface ITagRepository : IRepositoryAsync<Tag, int>
    {
        Task<int> SetArticleTagsAsync(int articleId, string[] tags);
    }
}