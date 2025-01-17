
using Editlio.Infrastructure.Repositories.Abstracts;
using Editlio.Shared.Entities;
using Editlio.Shared.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Editlio.Infrastructure.Repositories.Concretes
{
    public class EfRepositoryBase<TEntity, TEntityId, TContext> : IAsyncRepository<TEntity, TEntityId>
        where TEntity : Entity<TEntityId>
        where TContext : DbContext
    {
        private readonly TContext _context;
        private readonly DbSet<TEntity> _dbSet;

        public EfRepositoryBase(TContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }

        public async Task<Result<TEntity>> AddAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return Result<TEntity>.SuccessResult(entity);
        }

        public async Task<Result<ICollection<TEntity>>> AddRangeAsync(ICollection<TEntity> entities)
        {
            await _dbSet.AddRangeAsync(entities);
            await _context.SaveChangesAsync();
            return Result<ICollection<TEntity>>.SuccessResult(entities);
        }

        public async Task<Result<bool>> AnyAsync(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken cancellationToken = default)
        {
            var exists = predicate == null ? await _dbSet.AnyAsync(cancellationToken) : await _dbSet.AnyAsync(predicate, cancellationToken);
            return Result<bool>.SuccessResult(exists);
        }

        public async Task<Result> DeleteAsync(TEntity entity, bool permanent = true)
        {
            //if (permanent)
            //{
                _dbSet.Remove(entity);
            //}
            //else
            //{
            //    entity.DeletedDate = DateTime.UtcNow;
            //    _dbSet.Update(entity);
            //}

            await _context.SaveChangesAsync();
            return Result.SuccessResult();
        }

        public async Task<Result<TEntity?>> GetAsync(Expression<Func<TEntity, bool>> predicate, bool enableTracking = true, CancellationToken cancellationToken = default)
        {
            var query = enableTracking ? _dbSet : _dbSet.AsNoTracking();
            var entity = await query.FirstOrDefaultAsync(predicate, cancellationToken);
            return entity != null ? Result<TEntity?>.SuccessResult(entity) : Result<TEntity?>.FailureResult(new List<string> { "Entity not found" });
        }

        public async Task<Result<List<TEntity>>> GetListAsync(
            Expression<Func<TEntity, bool>>? predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            bool enableTracking = true,
            CancellationToken cancellationToken = default)
        {
            var query = enableTracking ? _dbSet : _dbSet.AsNoTracking();

            if (predicate != null)
                query = query.Where(predicate);

            if (orderBy != null)
                query = orderBy(query);

            var entities = await query.ToListAsync(cancellationToken);
            return Result<List<TEntity>>.SuccessResult(entities);
        }

        public async Task<Result<TEntity>> UpdateAsync(TEntity entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
            return Result<TEntity>.SuccessResult(entity);
        }
    }
}
