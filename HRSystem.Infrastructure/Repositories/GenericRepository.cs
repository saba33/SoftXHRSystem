using HRSystem.Application.Interfaces.Repositories;
using HRSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace HRSystem.Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly HRDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(HRDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<List<T>> GetAllAsync()
            => await _dbSet.ToListAsync();

        public async Task<T> GetByIdAsync(int id)
            => await _dbSet.FindAsync(id);

        public async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
            => await _dbSet.AnyAsync(predicate);

        public IQueryable<T> Query()
            => _dbSet.AsQueryable();
    }
}
