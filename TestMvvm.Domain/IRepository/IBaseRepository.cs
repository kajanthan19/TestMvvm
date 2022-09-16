using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TestMvvm.Domain.IRepository
{
    public interface IBaseRepository<T> where T : class
    {
        Task<IList<T>> GetAll();
        Task<T> Get(int id);
        Task<T> Add(T entity);
        void Update(T entity);
        Task DeleteAsync(object id);
        Task<IList<T>> GetAll(Expression<Func<T, bool>> searchBy);
        Task<bool> SetIsDeleteTrue(T entity);
    }
}
