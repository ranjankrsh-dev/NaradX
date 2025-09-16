using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaradX.Business.Tenants.CreateTenant
{
    public class CreateTenantCommand : IRequest<CreateTenantResponse>
    {
        public string Name { get; set; } = string.Empty;
        public string Subdomain { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? PhoneNumber { get; set; }
        public string? ContactEmail { get; set; }
        public string? Address { get; set; }
        public int MaxUsers { get; set; } = 5;
        public int MaxMessagesPerMonth { get; set; } = 10000;
    }
}
