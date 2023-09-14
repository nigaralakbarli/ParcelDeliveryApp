using Shared.Models;
using System.Linq.Expressions;

namespace DeliveryMicroservice.Repositories.Abstraction;

public interface IBaseRepository<TEntity> where TEntity : EntityBase
{
    Task<TEntity> GetByIdAsync(int id);
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> expression);
    Task AddAsync(TEntity entity);
    Task UpdateAsync(TEntity entity);
    Task UpdateRangeAsync(IEnumerable<TEntity> entities);
    Task AddRangeAsync(IEnumerable<TEntity> entities);
    Task RemoveAsync(TEntity entity);
    Task RemoveRangeAsync(IEnumerable<TEntity> entities);
}
