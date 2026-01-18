using MediatR;
using Microsoft.Extensions.Logging;
using NaradX.Domain.Entities.Tenancy;
using NaradX.Domain.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaradX.Business.Tenants.CreateTenant
{
    public class CreateTenantCommandHandler : IRequestHandler<CreateTenantCommand, CreateTenantResponse>
    {
        private readonly ITenantRepository _tenantRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CreateTenantCommandHandler> _logger;

        public CreateTenantCommandHandler(
            ITenantRepository tenantRepository,
            IUnitOfWork unitOfWork,
            ILogger<CreateTenantCommandHandler> logger)
        {
            _tenantRepository = tenantRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<CreateTenantResponse> Handle(CreateTenantCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Check if subdomain already exists
                if (await _tenantRepository.SubdomainExistsAsync(request.Subdomain, cancellationToken))
                {
                    throw new ApplicationException("Subdomain already exists");
                }

                // Create tenant entity
                var tenant = new Tenant
                {
                    Name = request.Name,
                    Domain = request.Subdomain,
                    Description = request.Description,
                    PhoneNumber = request.PhoneNumber,
                    ContactEmail = request.ContactEmail,
                    Address = request.Address,
                    MaxUsers = request.MaxUsers,
                    MaxMessagesPerMonth = request.MaxMessagesPerMonth,
                    SubscriptionStartDate = DateTime.UtcNow,
                    SubscriptionEndDate = DateTime.UtcNow.AddYears(1),
                    IsActive = true,
                    CreatedOn = DateTime.UtcNow
                };

                // Add tenant to database
                await _tenantRepository.AddAsync(tenant, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("Tenant created successfully: {Name}", request.Name);

                return new CreateTenantResponse
                {
                    TenantId = tenant.Id,
                    Name = tenant.Name,
                    Message = "Tenant created successfully"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating tenant: {Name}", request.Name);
                throw;
            }
        }
    }
}
