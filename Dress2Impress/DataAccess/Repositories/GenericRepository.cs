using Dress2Impress.DataAccess.IRepositories;
using Dress2Impress.Domain.DBContext;
using Microsoft.EntityFrameworkCore;

namespace Dress2Impress.DataAccess.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    protected readonly Dress2ImpressContext _context;
    protected readonly DbSet<T> _dbSet;

    public GenericRepository(Dress2ImpressContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.AsNoTracking().ToListAsync();
    }

    public async Task<T?> GetByIdAsync(object id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task InsertAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public Task UpdateAsync(T entity)
    {
        _dbSet.Attach(entity);
        _context.Entry(entity).State = EntityState.Modified;
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(object id)
    {
        var entity = await _dbSet.FindAsync(id) ?? throw new KeyNotFoundException("Entity not found");
        _dbSet.Remove(entity);
    }
}
