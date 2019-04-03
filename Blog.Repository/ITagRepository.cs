#region

using Blog.Entity;
using SmartSql.DyRepository;
using System.Threading.Tasks;

#endregion

namespace Blog.Repository
{
    public interface ITagRepository : IRepositoryAsync<Tag, int>
    {
        Task SetArticleTagsAsync(int articleId, string[] tags);
    }
}