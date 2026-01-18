using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NaradX.Domain.Repositories.Interfaces;
using NaradX.Infrastructure;
using System;

namespace NaradX.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public TestController(
            IRoleRepository roleRepository,
            IUserRepository userRepository,
            IUnitOfWork unitOfWork)
        {
            _roleRepository = roleRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        [HttpPost("assign-role")]
        public async Task<IActionResult> AssignRole([FromBody] AssignRoleRequest request)
        {
            try
            {
                var success = await _roleRepository.AssignRoleToUserAsync(request.UserId, request.RoleId);
                if (!success)
                    return BadRequest(new { Message = "Failed to assign role" });

                await _unitOfWork.SaveChangesAsync();
                return Ok(new { Message = "Role assigned successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error assigning role" });
            }
        }

        [HttpGet("users-with-roles")]
        public async Task<IActionResult> GetUsersWithRoles()
        {
            try
            {
                var users = await _userRepository.GetAllAsync();
                var result = new List<object>();

                foreach (var user in users)
                {
                    var roles = await _roleRepository.GetUserRolesAsync(user.Id);
                    result.Add(new
                    {
                        user.Id,
                        user.Email,
                        user.FirstName,
                        user.LastName,
                        Roles = roles.Select(r => r.Name)
                    });
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error retrieving users with roles" });
            }
        }
    }

    public class AssignRoleRequest
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
    }
}
