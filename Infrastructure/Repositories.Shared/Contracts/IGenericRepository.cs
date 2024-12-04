using System.Linq.Expressions;

namespace Infrastructure.Repositories.Shared.Contracts;
public interface IGenericRepository<T> where T : class
{
    Task<ICollection<T>> GetAllAsync();
    Task<T?> GetByIdAsync(int id);
    IQueryable<T> Find(Expression<Func<T, bool>> predicate);
    Task<T> AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
}
