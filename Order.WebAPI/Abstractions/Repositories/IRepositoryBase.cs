using System.Linq.Expressions;

namespace Order.WebAPI.Abstractions.Repositories;

public interface IRepositoryBase<T> where T : class
{
    Task<IEnumerable<T>> FindAllAsync();
    Task<IEnumerable<T>> FindByConditionAsync(Expression<Func<T, bool>> expression);
    Task<T?> FindByIdAsync(int id);
    Task<T> CreateAsync(T entity);
    Task<T> UpdateAsync(T entity);
    Task DeleteAsync(T entity);
    Task<bool> ExistsAsync(int id);
    Task SaveAsync();
}