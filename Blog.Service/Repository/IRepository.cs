using System.Linq;

namespace Blog.Service.Repository
{
    public interface IRepository<T>
    {
        void Add(T item);
        IQueryable<T> All();
        T Get(int id);
        void Delete(T item);
        void Update(T item);
        void Save();
    }
}
