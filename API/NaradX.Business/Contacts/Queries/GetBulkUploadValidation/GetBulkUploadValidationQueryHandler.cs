using ClosedXML.Excel;
using MediatR;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using NaradX.Business.Common.Services;
using NaradX.Domain.Entities.ManageContact;
using NaradX.Domain.Repositories.Interfaces;
using NaradX.Shared.Dto.BulkUpload;
using NaradX.Shared.Dto.Contact;
using NaradX.Shared.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaradX.Business.Contacts.Queries.GetBulkUploadValidation
{
    public class GetBulkUploadValidationQueryHandler : IRequestHandler<GetBulkUploadValidationQuery, BulkUploadValidateResponse>
    {
        private const int CACHE_EXPIRATION_MINUTES = 30;
        private readonly IMemoryCache memoryCache;
        private readonly ILogger<GetBulkUploadValidationQueryHandler> logger;
        private readonly IRepository<Contact> contactRepository;
        public GetBulkUploadValidationQueryHandler(IMemoryCache memoryCache, ILogger<GetBulkUploadValidationQueryHandler> logger, IRepository<Contact> contactRepository)
        {
            this.memoryCache = memoryCache;
            this.logger = logger;
            this.contactRepository = contactRepository;
        }
        public async Task<BulkUploadValidateResponse> Handle(GetBulkUploadValidationQuery request, CancellationToken cancellationToken)
        {
            var response = new BulkUploadValidateResponse();

            // Read and parse Excel file
            var contactRows = ExcelHelper.ReadExcelFile(request.UploadedFile.OpenReadStream(), MapExcelRowToContact);
            response.TotalRows = contactRows.Count;

            var validContacts = new List<ContactDto>();
            var invalidRows = new List<InvalidRowDto>();

            var uploadedPhoneNumbers = new HashSet<string>();
            var uploadedEmails = new HashSet<string>();

            foreach (var contactRow in contactRows)
            {
                var validationResult = await ValidateContact(contactRow, uploadedPhoneNumbers, uploadedEmails);

                if (validationResult.IsValid)
                {

                    if (!string.IsNullOrWhiteSpace(contactRow.PhoneNumber))
                        uploadedPhoneNumbers.Add(contactRow.PhoneNumber);

                    if (!string.IsNullOrWhiteSpace(contactRow.Email))
                        uploadedEmails.Add(contactRow.Email);

                    validContacts.Add(new ContactDto
                    {
                        TenantId = request.TenantId,
                        FirstName = contactRow.FirstName,
                        MiddleName = contactRow.MiddleName,
                        LastName = contactRow.LastName,
                        CountryId = request.CountryId,
                        LanguageId = request.LanguageId,
                        PhoneNumber = contactRow.PhoneNumber,
                        ContactSource = request.ContactSource,
                        ChannelPreference = request.ChannelPreference,
                        Email = contactRow.Email,
                        Company = contactRow.Company,
                        JobTitle = contactRow.JobTitle
                    });
                }
                else
                {
                    invalidRows.Add(new InvalidRowDto
                    {
                        RowNumber = contactRow.RowNumber,
                        FirstName = contactRow.FirstName,
                        LastName = contactRow.LastName,
                        PhoneNumber = contactRow.PhoneNumber,
                        Errors = validationResult.Errors
                    });
                }
            }

            response.ValidRowsCount = validContacts.Count;
            response.InvalidRowsCount = invalidRows.Count;
            response.InvalidRows = invalidRows;

            if (validContacts.Any())
            {
                var batchId = Guid.NewGuid().ToString();
                response.BatchId = batchId;

                var batch = new BulkUploadBatch
                {
                    BatchId = batchId,
                    ValidContacts = validContacts,
                    CreatedAt = DateTime.UtcNow
                };

                memoryCache.Set(batchId, batch, TimeSpan.FromMinutes(CACHE_EXPIRATION_MINUTES));
                response.Message = $"Validation complete: {response.ValidRowsCount} valid, {response.InvalidRowsCount} invalid rows.";
            }
            else
            {
                response.Message = "No valid contacts found. Please fix errors and re-upload.";
            }

            return response;
        }

        private ContactTemplateDto MapExcelRowToContact(IXLRangeRow row, int rowNumber)
        {
            return new ContactTemplateDto
            {
                RowNumber = rowNumber,
                FirstName = row.Cell(1).GetValue<string>().Trim(),
                MiddleName = row.Cell(2).GetValue<string>()?.Trim(),
                LastName = row.Cell(3).GetValue<string>().Trim(),
                PhoneNumber = row.Cell(4).GetValue<string>().Trim(),
                Email = row.Cell(5).GetValue<string>()?.Trim(),
                Company = row.Cell(6).GetValue<string>()?.Trim(),
                JobTitle = row.Cell(7).GetValue<string>()?.Trim()
            };
        }

        private async Task<ValidationResultDto> ValidateContact(ContactTemplateDto contact, HashSet<string> uploadedPhoneNumbers, HashSet<string> uploadedEmails)
        {
            var errors = new List<string>();

            // Required field validation
            if (string.IsNullOrWhiteSpace(contact.FirstName))
                errors.Add("First Name is required");

            if (string.IsNullOrWhiteSpace(contact.LastName))
                errors.Add("Last Name is required");

            if (string.IsNullOrWhiteSpace(contact.PhoneNumber))
                errors.Add("Phone Number is required");

            // Phone number format validation
            if (!string.IsNullOrWhiteSpace(contact.PhoneNumber) && !IsValidPhoneNumber(contact.PhoneNumber))
                errors.Add("Phone Number is not in valid format");

            // Check for duplicate phone number within the uploaded file
            if (!string.IsNullOrWhiteSpace(contact.PhoneNumber) && uploadedPhoneNumbers.Contains(contact.PhoneNumber))
                errors.Add("Phone number is duplicated within the uploaded file");

            // Duplicate phone number check
            if (!string.IsNullOrWhiteSpace(contact.PhoneNumber) &&
                await contactRepository.AnyAsync(c => c.PhoneNumber == contact.PhoneNumber))
                errors.Add("Phone number already exists");

            // Email format validation (if provided)
            if (!string.IsNullOrWhiteSpace(contact.Email) && !IsValidEmail(contact.Email))
                errors.Add("Invalid email format");

            // Check for duplicate email within the uploaded file
            if (!string.IsNullOrWhiteSpace(contact.Email) && uploadedEmails.Contains(contact.Email))
                errors.Add("Email is duplicated within the uploaded file");

            if (!string.IsNullOrWhiteSpace(contact.Email) &&
                await contactRepository.AnyAsync(c => c.Email == contact.Email))
                errors.Add("Email already exists");

            return new ValidationResultDto { IsValid = !errors.Any(), Errors = errors };
        }

        private bool IsValidPhoneNumber(string phoneNumber)
        {
            return !string.IsNullOrWhiteSpace(phoneNumber) && phoneNumber.Length >= 10 && phoneNumber.All(char.IsDigit);
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var mailAddress = new System.Net.Mail.MailAddress(email);
                return mailAddress.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
