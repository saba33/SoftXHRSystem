using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace HRSystem.Application.Interfaces.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetByIdAsync(int id);
        Task<List<T>> GetAllAsync();
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
        IQueryable<T> Query();
    }
}
