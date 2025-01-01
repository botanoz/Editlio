using Editlio.Shared.Entities;
using Editlio.Shared.Helpers;
using System.Linq.Expressions;


namespace Editlio.Infrastructure.Repositories.Abstracts
{
    public interface IAsyncRepository<TEntity, TEntityId> where TEntity : IEntity
    {
        Task<Result<TEntity>> AddAsync(TEntity entity);
        Task<Result<ICollection<TEntity>>> AddRangeAsync(ICollection<TEntity> entities);
        Task<Result<TEntity?>> GetAsync(
            Expression<Func<TEntity, bool>> predicate,
            bool enableTracking = true,
            CancellationToken cancellationToken = default);
        Task<Result<List<TEntity>>> GetListAsync(
            Expression<Func<TEntity, bool>>? predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            bool enableTracking = true,
            CancellationToken cancellationToken = default);
        Task<Result<TEntity>> UpdateAsync(TEntity entity);
        Task<Result> DeleteAsync(TEntity entity, bool permanent = false);
        Task<Result<bool>> AnyAsync(
            Expression<Func<TEntity, bool>>? predicate = null,
            CancellationToken cancellationToken = default);
    }
}
