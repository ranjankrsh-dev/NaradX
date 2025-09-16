using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NaradX.Domain.Repositories.Interfaces;
using System.Security.Claims;

namespace NaradX.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetCurrentUser()
        {
            try
            {
                // Get user ID from JWT token claims
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var user = await _userRepository.GetUserWithTenantAsync(userId);

                if (user == null)
                    return NotFound(new { Message = "User not found" });

                return Ok(new
                {
                    user.Id,
                    user.Email,
                    user.FirstName,
                    user.LastName,
                    user.PhoneNumber,
                    Tenant = new { user.Tenant.Id, user.Tenant.Name }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error retrieving user data" });
            }
        }

        [HttpGet]
        [Authorize(Roles = "TenantAdmin,SuperAdmin")] // Role-based authorization
        public async Task<IActionResult> GetUsers([FromQuery] int tenantId)
        {
            try
            {
                // Verify user has access to the requested tenant
                var currentUserTenantId = int.Parse(User.FindFirst("tenantId")?.Value);

                if (currentUserTenantId != tenantId && !User.IsInRole("SuperAdmin"))
                    return Forbid(); // User cannot access other tenants' data

                var users = await _userRepository.GetUsersByTenantAsync(tenantId);
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error retrieving users" });
            }
        }

        [HttpPut("{id}/deactivate")]
        [Authorize(Roles = "TenantAdmin,SuperAdmin")]
        public async Task<IActionResult> DeactivateUser(int id, [FromBody] string deactivatedBy)
        {
            try
            {
                var success = await _userRepository.DeactivateUserAsync(id, deactivatedBy);

                if (!success)
                    return NotFound(new { Message = "User not found" });

                return Ok(new { Message = "User deactivated successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error deactivating user" });
            }
        }
    }
}
