#region

using System.Threading.Tasks;
using Blog.API.Messages;
using Blog.Entity;
using Blog.Repository;

#endregion

namespace Blog.Service
{
    public class ArticleService
    {
        public ArticleService(IArticleRepository articleRepository)
        {
            ArticleRepository = articleRepository;
        }

        public IArticleRepository ArticleRepository { get; }

        public async Task<int> Insert(Article article)
        {
            return await ArticleRepository.InsertAsync(article);
        }

        public async Task<int> Update(Article article)
        {
            return await ArticleRepository.UpdateAsync(article);
        }

        public async Task<QueryByPageResponse<Article>> QueryByPageAsync(QueryByPageParameter request)
        {
            var result = await ArticleRepository.QueryByPageAsync<QueryByPageResponse<Article>>(request);
            result.PageIndex = request.PageIndex;
            result.PageSize = request.PageSize;
            return result;
        }
    }
}