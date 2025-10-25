using MediatR;
using Microsoft.AspNetCore.Mvc;
using NaradX.Business.Template.Commands;
using NaradX.Shared.Dto.Template;

namespace NaradX.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TemplateController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpPost]
    public async Task<IActionResult> CreateTemplate([FromBody] WhatsAppMessageTemplateDTO whatsappTemplate)
    {
        var whatsappTemplateCommand = new CreateTemplateCommand(whatsappTemplate);
        var result = await _mediator.Send(whatsappTemplateCommand);
        return Ok(result);
    }
}
