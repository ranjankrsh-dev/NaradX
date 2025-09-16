using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NaradX.Business.Auth.Roles.AssignRole;
using NaradX.Business.Auth.Roles.GetUserRoles;
using NaradX.Business.Auth.Roles.RemoveRole;
using NaradX.Shared.Dto.Role;

namespace NaradX.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize(Roles = "SuperAdmin")]
    public class RolesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RolesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("assign")]
        public async Task<IActionResult> AssignRole([FromBody] AssignRoleRequest request)
        {
            try
            {
                var assignedBy = User.Identity?.Name ?? "Unknown";

                var command = new AssignRoleCommand
                {
                    UserId = request.UserId,
                    RoleId = request.RoleId,
                    AssignedBy = assignedBy
                };

                var result = await _mediator.Send(command);

                if (!result.Success)
                    return BadRequest(new { result.Message });

                return Ok(new { result.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error assigning role" });
            }
        }

        [HttpPost("remove")]
        public async Task<IActionResult> RemoveRole([FromBody] RemoveRoleRequest request)
        {
            try
            {
                var removedBy = User.Identity?.Name ?? "Unknown";

                var command = new RemoveRoleCommand
                {
                    UserId = request.UserId,
                    RoleId = request.RoleId,
                    RemovedBy = removedBy
                };

                var result = await _mediator.Send(command);

                if (!result.Success)
                    return BadRequest(new { result.Message });

                return Ok(new { result.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error removing role" });
            }
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserRoles(int userId)
        {
            try
            {
                var query = new GetUserRolesQuery { UserId = userId };
                var result = await _mediator.Send(query);
                return Ok(result);
            }
            catch (ApplicationException ex)
            {
                return NotFound(new { ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error retrieving user roles" });
            }
        }
    }
}
