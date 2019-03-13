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

        public int Insert(ArticleCategories articleCategories)
        {
            return ArticleCategoriesRepository.Insert(articleCategories);
        }

        public int DeleteById(int id)
        {
            return ArticleCategoriesRepository.DeleteById(id);
        }

        public int Update(ArticleCategories articleCategories)
        {
            return ArticleCategoriesRepository.Update(articleCategories);
        }
    }
}