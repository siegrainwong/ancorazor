#region

using Blog.Repository;

#endregion

namespace Blog.Service
{
    public class ArticleCategoriesService
    {
        public IArticleCategoriesRepository ArticleCategoriesRepository { get; }

        public ArticleCategoriesService(IArticleCategoriesRepository articleCategoriesRepository)
        {
            ArticleCategoriesRepository = articleCategoriesRepository;
        }
    }
}