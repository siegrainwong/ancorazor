#region

using Blog.Entity;
using Blog.Repository;

#endregion

namespace Blog.Service
{
    public class ArticleCategoriesService
    {
        public ArticleCategoriesService(IArticleCategoriesRepository articleCategoriesRepository)
        {
            ArticleCategoriesRepository = articleCategoriesRepository;
        }

        public IArticleCategoriesRepository ArticleCategoriesRepository { get; }

    }
}