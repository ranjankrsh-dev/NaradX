// <copyright file="IUnitOfWork.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace NaradX.Domain.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using NaradX.Domain.Entities.Auth;
    using NaradX.Domain.Entities.Base;

    public interface IUnitOfWork : IDisposable
    {
        // Repository Properties
        IUserRepository Users { get; }

        ITenantRepository Tenants { get; }

        IRoleRepository Roles { get; }

        IRepository<UserRole> UserRoles { get; }

        // Transaction Management
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        Task BeginTransactionAsync(CancellationToken cancellationToken = default);

        Task CommitTransactionAsync(CancellationToken cancellationToken = default);

        Task RollbackTransactionAsync(CancellationToken cancellationToken = default);

        // Generic Repository Access
        IRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity<int>;
    }
}
