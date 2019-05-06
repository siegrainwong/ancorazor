using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Blog.EF.Entity;
using Blog.EF.Entity.Base;
using Microsoft.EntityFrameworkCore;

namespace Blog.Service.Repository
{
    public class BaseRepository<T> : IRepository<T> where T: BaseEntity
    {
        private readonly BlogContext _context;
        private readonly DbSet<T> _entities;

        public BaseRepository(BlogContext context)
        {
            _context = context;
            _entities = context.Set<T>();
        }

        public IQueryable<T> All()
        {
            return _entities;
        }

        public T Get(int id)
        {
            return _entities.Find(id);
        }

        public void Add(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            entity.CreatedAt = DateTime.Now;
            entity.UpdatedAt = DateTime.Now;

            _entities.Add(entity);
        }

        public void Update(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            entity.UpdatedAt = DateTime.Now;
        }

        public void Delete(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            _entities.Remove(entity);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
