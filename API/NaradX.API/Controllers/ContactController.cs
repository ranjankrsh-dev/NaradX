using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NaradX.Business.Common.Interfaces;
using NaradX.Business.Contacts.Commands.CreateContact;
using NaradX.Business.Contacts.Commands.DeleteContact;
using NaradX.Business.Contacts.Commands.UpdateContact;
using NaradX.Business.Contacts.Queries.GetContactById;
using NaradX.Business.Contacts.Queries.GetContacts;
using NaradX.Shared.Common.Model;
using NaradX.Shared.Dto.Contact;

namespace NaradX.API.Controllers
{
    [Route("api/[controller]")]
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

        [HttpGet]
        public async Task<ActionResult<PaginatedList<ContactDto>>> GetContacts(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? searchTerm = null,
            [FromQuery] string? tag = null)
        {
            var query = new GetContactsQuery
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                SearchTerm = searchTerm,
                Tag = tag
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

        [HttpPost]
        public async Task<ActionResult<int>> CreateContact(CreateContactDto dto)
        {
            var command = new CreateContactCommand
            {
                ContactName = dto.ContactName,
                PhoneNumber = dto.PhoneNumber,
                Email = dto.Email,
                Company = dto.Company,
                Title = dto.Title,
                Tags = dto.Tags,
                LanguagePreference = dto.LanguagePreference,
                Timezone = dto.Timezone
            };

            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetContact), new { id = result }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateContact(int id, CreateContactDto dto)
        {
            var command = new UpdateContactCommand
            {
                Id = id,
                ContactName = dto.ContactName,
                PhoneNumber = dto.PhoneNumber,
                Email = dto.Email,
                Company = dto.Company,
                Title = dto.Title,
                Tags = dto.Tags,
                LanguagePreference = dto.LanguagePreference,
                Timezone = dto.Timezone
            };

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
