using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TestMvvm.Domain.IRepository;

namespace TestMvvm.Infrastructure.BaseRepository
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly DbContext _context;
        private readonly DbSet<T> _dbSet;

        public BaseRepository(DbContext context)
        {
            this._context = context;
            this._dbSet = context.Set<T>();
        }

        public async Task<T> Get(int id)
        {
            var x = await _dbSet.FindAsync(id);
            return x;
        }

        public virtual async Task<IList<T>> GetAll()
        {
            var result = _context.Set<T>().Where(i => true);
            return await result.ToListAsync();
        }

        public async Task<T> Add(T entity)
        {
            await _context.AddAsync(entity);
            return entity;
        }

        public void Update(T entity)
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        public async Task DeleteAsync(object id)
        {
            var entity = await Get((int)id);
            if (entity != null)
            {
                if (_context.Entry(entity).State == EntityState.Detached)
                {
                    _dbSet.Attach(entity);
                }
                _dbSet.Remove(entity);
            }
        }

        public async Task<IList<T>> GetAll(Expression<Func<T, bool>> searchBy)
        {
            var result = _context.Set<T>().Where(searchBy).AsNoTracking();

            return await result.ToListAsync();
        }


        public virtual async Task<bool> SetIsDeleteTrue(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            return await Task.FromResult(true);
        }
    }
}
