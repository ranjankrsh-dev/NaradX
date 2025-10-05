using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NaradX.Business.Common.Interfaces;
using NaradX.Business.Contacts.Commands.BulkUploadContact;
using NaradX.Business.Contacts.Commands.CreateContact;
using NaradX.Business.Contacts.Commands.DeleteContact;
using NaradX.Business.Contacts.Commands.UpdateContact;
using NaradX.Business.Contacts.Queries.GetBulkUploadValidation;
using NaradX.Business.Contacts.Queries.GetContactById;
using NaradX.Business.Contacts.Queries.GetContacts;
using NaradX.Shared.Dto.BulkUpload;
using NaradX.Shared.Dto.Common;
using NaradX.Shared.Dto.Contact;
using NaradX.Shared.Helpers;
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
        private readonly IWebHostEnvironment _environment;

        public ContactController(IMediator mediator,ILogger<ContactController> logger, IWebHostEnvironment environment)
        {
            _mediator = mediator;
            _logger = logger;
            _environment = environment;
        }

        [HttpPost("list")]
        public async Task<ActionResult<PaginatedList<ContactDto>>> GetContacts([FromBody] ContactFilterParams filterParams)
        {
            var query = new GetContactsQuery(filterParams);

            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost("add")]
        public async Task<ActionResult<int>> CreateContact(CreateContactCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateContact([FromBody] UpdateContactCommand command)
        {
            var result = await _mediator.Send(command);
            if (result == 0) {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpGet("get-contact-by-id/{Id}")]
        public async Task<ActionResult<ContactDto>> GetContactById(int Id)
        {
            var query = new GetContactByIdQuery { Id = Id };
            var result = await _mediator.Send(query);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpDelete("delete-contact-by-id/{Id}")]
        public async Task<IActionResult> DeleteContact(int Id)
        {
            var resp = await _mediator.Send(new DeleteContactCommand { Id = Id });
            if (resp == 0)
                return NotFound();

            return Ok(resp);
        }

        #region Bulk Operations

        [HttpPost("bulk-upload-validate")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<BulkUploadValidateResponse>> ValidateBulkUpload([FromForm] GetBulkUploadValidationQuery query)
        {
            if (query.UploadedFile == null || query.UploadedFile.Length == 0)
                return BadRequest(new BulkUploadValidateResponse { Message = "Please select a file" });

            if (!ExcelHelper.IsValidExcelFile(query.UploadedFile))
                return BadRequest(new BulkUploadValidateResponse { Message = "Please upload a valid Excel file (.xlsx, .xls)" });

            using (var stream = query.UploadedFile.OpenReadStream())
            {
                if (!ExcelHelper.HasDataRows(stream))
                {
                    return BadRequest(new BulkUploadValidateResponse { Message = "The Excel file is empty or contains only headers. Please add data rows." });
                }
            }

            var result = await _mediator.Send(query);

            return Ok(result);
        }

        [HttpPost("bulk-upload-confirm")]
        public async Task<ActionResult> ConfirmBulkUpload([FromBody] BulkUploadContactCommand command)
        {
            if (string.IsNullOrWhiteSpace(command.BatchId))
                return BadRequest(new ResponseDto { IsSuccess = false, Message = "Batch ID is required" });

            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("bulk-upload-download-template")]
        public IActionResult DownloadTemplate()
        {
            var templatePath = Path.Combine(_environment.ContentRootPath, "Common", "BulkUploadTemplate", "Bulk Upload Contact Template.xlsx");

            if (!System.IO.File.Exists(templatePath))
            {
                return NotFound(new ResponseDto { IsSuccess = false, Message = "Template file not found" });
            }

            var fileBytes = System.IO.File.ReadAllBytes(templatePath);
            return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Bulk Upload Contact Template.xlsx");
        }
        #endregion
    }
}
