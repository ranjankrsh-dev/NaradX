using NaradX.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NaradX.Domain.Repositories.Interfaces
{
    public interface IRepository<TEntity> where TEntity : BaseEntity<int>
    {
        #region Read Operations

        Task<TEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<TEntity?> GetByIdAsync(int id, params Expression<Func<TEntity, object>>[] includes);

        Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<TEntity>> GetAllAsync(params Expression<Func<TEntity, object>>[] includes);

        Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
        Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes);

        Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
        Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes);

        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
        Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

        #endregion

        #region Write Operations

        Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);
        Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

        void Update(TEntity entity);
        void UpdateRange(IEnumerable<TEntity> entities);

        void Remove(TEntity entity);
        void RemoveRange(IEnumerable<TEntity> entities);

        Task RemoveByIdAsync(int id, CancellationToken cancellationToken = default);

        #endregion

        #region Special Operations

        Task<IEnumerable<TEntity>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);
        Task<IEnumerable<TEntity>> GetPagedAsync(int pageNumber, int pageSize, Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

        Task<IEnumerable<TEntity>> FromSqlRawAsync(string sql, params object[] parameters);

        #endregion
    }
}
