#region

using Blog.Entity;
using Blog.Repository;

#endregion

namespace Blog.Service
{
    public class CategoryService
    {
        public CategoryService(ICategoryRepository categoryRepository)
        {
            CategoryRepository = categoryRepository;
        }

        public ICategoryRepository CategoryRepository { get; }

        public int Insert(Category category)
        {
            return CategoryRepository.Insert(category);
        }

        public int DeleteById(int id)
        {
            return CategoryRepository.DeleteById(id);
        }

        public int Update(Category category)
        {
            return CategoryRepository.Update(category);
        }
    }
}