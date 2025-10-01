using Inventory.WebAPI.Abstractions.Repositories;
using Inventory.WebAPI.Data;
using Inventory.WebAPI.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Inventory.WebAPI.Repositories;

public class RepositoryBase<T> : IRepositoryBase<T> where T : BaseEntity
{
    protected ApplicationDbContext Context { get; set; }

    public RepositoryBase(ApplicationDbContext context)
    {
        Context = context;
    }

    public async Task<IEnumerable<T>> FindAllAsync()
    {
        return await Context.Set<T>().ToListAsync();
    }

    public async Task<IEnumerable<T>> FindByConditionAsync(Expression<Func<T, bool>> expression)
    {
        return await Context.Set<T>().Where(expression).ToListAsync();
    }

    public async Task<T?> FindByIdAsync(int id)
    {
        return await Context.Set<T>().FindAsync(id);
    }

    public async Task<T> CreateAsync(T entity)
    {
        var result = await Context.Set<T>().AddAsync(entity);
        await SaveAsync();
        return result.Entity;
    }

    public async Task<T> UpdateAsync(T entity)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        Context.Set<T>().Update(entity);
        await SaveAsync();
        return entity;
    }

    public async Task DeleteAsync(T entity)
    {
        Context.Set<T>().Remove(entity);
        await SaveAsync();
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await Context.Set<T>().AnyAsync(e => e.Id == id);
    }

    public async Task SaveAsync()
    {
        await Context.SaveChangesAsync();
    }
}