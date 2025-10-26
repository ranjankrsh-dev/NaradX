using MediatR;
using Microsoft.AspNetCore.Mvc;
using NaradX.Business.Template.Commands;
using NaradX.Business.Template.Query;
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

    [HttpGet]
    public async Task<IActionResult> GetAllTemplates()
    {
        return Ok(await _mediator.Send(new GetAllTemplateQuery()));
    }

    [HttpGet("{name}")]
    public async Task<IActionResult> GetTemplateByName(string name)
    {
        var query = new GetTemplateByNameQuery(name);
        var result = await _mediator.Send(query);
        if (result == null)
            return NotFound();
        return Ok(result);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteTemplate([FromBody] DeleteTemplateCommand command)
    {
        return Ok(await _mediator.Send(command));
    }
}
