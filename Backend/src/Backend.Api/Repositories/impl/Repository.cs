using Backend.Api.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Backend.Api.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AppDbContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync(Func<IQueryable<T>, IQueryable<T>>? include = null)
        {
            IQueryable<T> query = _dbSet.AsQueryable();
            if (include != null) query = include(query);
            return await query.ToListAsync();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            // Not: DbSet.FindAsync() Include uygulatmaz. Include gereken yerlerde FirstOrDefaultAsync kullan.
            return await _dbSet.FindAsync(id);
        }

        public async Task<IEnumerable<T>> FindAsync(
            Expression<Func<T, bool>> predicate,
            Func<IQueryable<T>, IQueryable<T>>? include = null)
        {
            IQueryable<T> query = _dbSet.Where(predicate);
            if (include != null) query = include(query);
            return await query.ToListAsync();
        }

        public async Task<T?> FirstOrDefaultAsync(
            Expression<Func<T, bool>> predicate,
            Func<IQueryable<T>, IQueryable<T>>? include = null)
        {
            IQueryable<T> query = _dbSet.Where(predicate);
            if (include != null) query = include(query);
            return await query.FirstOrDefaultAsync();
        }

        public Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.AnyAsync(predicate);
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
