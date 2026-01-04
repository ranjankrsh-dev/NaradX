using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NaradX.Business.Tenants.CreateTenant;
using NaradX.Domain.Repositories.Interfaces;

namespace NaradX.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize(Roles = "SuperAdmin")]
    public class TenantsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ITenantRepository _tenantRepository;

        public TenantsController(IMediator mediator, ITenantRepository tenantRepository)
        {
            _mediator = mediator;
            _tenantRepository = tenantRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTenant([FromBody] CreateTenantCommand command)
        {
            try
            {
                var result = await _mediator.Send(command);
                return Ok(result);
            }
            catch (ApplicationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Internal server error" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTenants()
        {
            try
            {
                var tenants = await _tenantRepository.GetAllAsync();
                return Ok(tenants);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error retrieving tenants" });
            }
        }

        [HttpPost("{id}/suspend")]
        public async Task<IActionResult> SuspendTenant(int id, [FromBody] string suspendedBy)
        {
            try
            {
                var success = await _tenantRepository.SuspendTenantAsync(id, suspendedBy);

                if (!success)
                    return NotFound(new { Message = "Tenant not found" });

                return Ok(new { Message = "Tenant suspended successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error suspending tenant" });
            }
        }

        [HttpGet("stats")]
        public async Task<IActionResult> GetTenantStats()
        {
            try
            {
                var activeTenants = await _tenantRepository.GetActiveTenantsCountAsync();
                var totalTenants = 10;// await _tenantRepository.GetTotalTenantsCountAsync();

                return Ok(new
                {
                    ActiveTenants = activeTenants,
                    TotalTenants = totalTenants,
                    SuspendedTenants = totalTenants - activeTenants
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error retrieving tenant stats" });
            }
        }
    }
}
