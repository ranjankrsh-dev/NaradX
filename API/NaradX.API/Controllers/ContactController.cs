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
using NaradX.Shared.Models.Contact;

namespace NaradX.API.Controllers
{
    [Route("api/contact")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ContactController> _logger;

        public ContactController(IMediator mediator,ILogger<ContactController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost("list")]
        public async Task<ActionResult<PaginatedList<ContactDto>>> GetContacts([FromBody] ContactFilterParams filterParams)
        {
            var query = new GetContactsQuery(filterParams);

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
