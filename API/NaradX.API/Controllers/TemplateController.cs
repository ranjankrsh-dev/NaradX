using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MediatR;
using NaradX.Business.Template.Commands;

namespace NaradX.API.Controllers;

[Route("api/[controller]")]
[ApiController]
//[Authorize]
public class TemplateController : ControllerBase
{
    private readonly IMediator _mediatr;
    public TemplateController(IMediator mediatr)
    {
        _mediatr = mediatr;
    }

    [HttpGet]
    //[Authorize(Roles = "TenantAdmin,SuperAdmin")] // Role-based authorization
    public async Task<IActionResult> CreateTemplate()
    {
        try
        {
            return Ok(await _mediatr.Send(new CreateTemplateCommand()));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "Error retrieving users" });
        }
    }
}
