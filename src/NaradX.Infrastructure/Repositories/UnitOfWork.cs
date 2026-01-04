using Microsoft.EntityFrameworkCore.Storage;
using NaradX.Domain.Entities.Auth;
using NaradX.Domain.Entities.Base;
using NaradX.Domain.Repositories.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaradX.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly NaradXDbContext _context;
        private IDbContextTransaction? _transaction;
        private bool _disposed = false;
        private readonly ConcurrentDictionary<Type, object> _repositories = new();

        public UnitOfWork(NaradXDbContext context)
        {
            _context = context;

            // Initialize specific repositories
            Users = new UserRepository(_context);
            Tenants = new TenantRepository(_context);
            Roles = new RoleRepository(_context);
            UserRoles = new Repository<UserRole>(_context);
        }

        // Repository Properties
        public IUserRepository Users { get; private set; }
        public ITenantRepository Tenants { get; private set; }
        public IRoleRepository Roles { get; private set; }
        public IRepository<UserRole> UserRoles { get; private set; }

        // Generic Repository Access
        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity<int>
        {
            var type = typeof(TEntity);

            if (!_repositories.ContainsKey(type))
            {
                var repository = new Repository<TEntity>(_context);
                _repositories.TryAdd(type, repository);
            }

            return (IRepository<TEntity>)_repositories[type];
        }

        // Transaction Management
        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_transaction != null)
            {
                throw new InvalidOperationException("A transaction is already in progress.");
            }

            _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        }

        public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_transaction == null)
            {
                throw new InvalidOperationException("No transaction to commit.");
            }

            try
            {
                await _context.SaveChangesAsync(cancellationToken);
                await _transaction.CommitAsync(cancellationToken);
            }
            catch
            {
                await RollbackTransactionAsync(cancellationToken);
                throw;
            }
            finally
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_transaction == null)
            {
                throw new InvalidOperationException("No transaction to rollback.");
            }

            try
            {
                await _transaction.RollbackAsync(cancellationToken);
            }
            finally
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        // Utility Methods
        public async Task ExecuteInTransactionAsync(Func<Task> action, CancellationToken cancellationToken = default)
        {
            await BeginTransactionAsync(cancellationToken);

            try
            {
                await action();
                await CommitTransactionAsync(cancellationToken);
            }
            catch
            {
                await RollbackTransactionAsync(cancellationToken);
                throw;
            }
        }

        public async Task<T> ExecuteInTransactionAsync<T>(Func<Task<T>> action, CancellationToken cancellationToken = default)
        {
            await BeginTransactionAsync(cancellationToken);

            try
            {
                var result = await action();
                await CommitTransactionAsync(cancellationToken);
                return result;
            }
            catch
            {
                await RollbackTransactionAsync(cancellationToken);
                throw;
            }
        }

        public bool HasActiveTransaction()
        {
            return _transaction != null;
        }

        // Dispose pattern
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _transaction?.Dispose();
                    _context.Dispose();
                }

                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Async dispose pattern (for .NET Core 3.0+)
        public async ValueTask DisposeAsync()
        {
            if (_transaction != null)
            {
                await _transaction.DisposeAsync();
            }

            await _context.DisposeAsync();
            Dispose(false);
            GC.SuppressFinalize(this);
        }
    }
}
