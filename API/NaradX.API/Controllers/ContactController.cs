using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NaradX.Business.Common.Interfaces;
using NaradX.Business.Contacts.Commands.CreateContact;
using NaradX.Business.Contacts.Commands.DeleteContact;
using NaradX.Business.Contacts.Commands.UpdateContact;
using NaradX.Business.Contacts.Queries.GetContactById;
using NaradX.Business.Contacts.Queries.GetContacts;
using NaradX.Shared.Dto.Contact;
using NaradX.Shared.Models.Common;

namespace NaradX.API.Controllers
{
    [Route("api/contact")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ContactController> _logger;
        private readonly IConfigValueService _configValueService;

        public ContactController(IMediator mediator,ILogger<ContactController> logger, IConfigValueService configValueService)
        {
            _mediator = mediator;
            _logger = logger;
            _configValueService = configValueService;
        }

        [HttpGet("list")]
        public async Task<ActionResult<PaginatedList<ContactDto>>> GetContacts(
            [FromQuery] int tenantId,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? searchTerm = null,
            [FromQuery] string? name = null,
            [FromQuery] string? phone = null,
            [FromQuery] string? status = null,
            [FromQuery] string? sortColumn = null,
            [FromQuery] string? sortDirection = "asc")
        {
            var query = new GetContactsQuery
            {
                TenantId = tenantId,
                PageNumber = pageNumber,
                PageSize = pageSize,
                SearchTerm = searchTerm,
                Name = name,
                Phone = phone,
                Status = status,
                SortColumn = sortColumn,
                SortDirection = sortDirection
            };

            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ContactDto>> GetContact(int id)
        {
            var query = new GetContactByIdQuery { Id = id };
            var result = await _mediator.Send(query);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpPost("add")]
        public async Task<ActionResult<int>> CreateContact(CreateContactCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateContact(UpdateContactCommand command)
        {
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContact(int id)
        {
            await _mediator.Send(new DeleteContactCommand { Id = id });
            return NoContent();
        }
    }
}
