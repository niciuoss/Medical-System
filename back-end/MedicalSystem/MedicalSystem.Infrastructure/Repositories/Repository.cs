using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using MedicalSystem.Domain.Interfaces;
using MedicalSystem.Infrastructure.Data;

namespace MedicalSystem.Infrastructure.Repositories
{
  public class Repository<T> : IRepository<T> where T : class
  {
    protected readonly MedicalDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public Repository(MedicalDbContext context)
    {
      _context = context;
      _dbSet = context.Set<T>();
    }

    public virtual async Task<T?> GetByIdAsync(Guid id)
    {
      return await _dbSet.FindAsync(id);
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
      return await _dbSet.ToListAsync();
    }

    public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
    {
      return await _dbSet.Where(predicate).ToListAsync();
    }

    public virtual async Task<T> CreateAsync(T entity)
    {
      await _dbSet.AddAsync(entity);
      await _context.SaveChangesAsync();
      return entity;
    }

    public virtual async Task<T> UpdateAsync(T entity)
    {
      _dbSet.Update(entity);
      await _context.SaveChangesAsync();
      return entity;
    }

    public virtual async Task DeleteAsync(Guid id)
    {
      var entity = await GetByIdAsync(id);
      if (entity != null)
      {
        _dbSet.Remove(entity);
        await _context.SaveChangesAsync();
      }
    }

    public virtual async Task<bool> ExistsAsync(Guid id)
    {
      return await _dbSet.FindAsync(id) != null;
    }
  }
}
