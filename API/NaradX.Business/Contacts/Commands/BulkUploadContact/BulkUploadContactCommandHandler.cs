// <copyright file="BulkUploadContactCommandHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace NaradX.Business.Contacts.Commands.BulkUploadContact
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using DocumentFormat.OpenXml.VariantTypes;
    using MediatR;
    using Microsoft.Extensions.Caching.Memory;
    using Microsoft.Extensions.Logging;
    using NaradX.Business.Dtos.BulkUpload;
    using NaradX.Business.Dtos.Common;
    using NaradX.Business.Dtos.Contact;
    using NaradX.Domain.Entities.ManageContact;
    using NaradX.Domain.Enums;
    using NaradX.Domain.Interfaces;

    public class BulkUploadContactCommandHandler : IRequestHandler<BulkUploadContactCommand, ResponseDto>
    {
        private readonly IMemoryCache memoryCache;
        private readonly ILogger<BulkUploadContactCommandHandler> logger;
        private readonly IContactRepository contactRepository;

        public BulkUploadContactCommandHandler(IMemoryCache memoryCache, ILogger<BulkUploadContactCommandHandler> logger, IContactRepository contactRepository)
        {
            this.memoryCache = memoryCache;
            this.logger = logger;
            this.contactRepository = contactRepository;
        }

        public async Task<ResponseDto> Handle(BulkUploadContactCommand request, CancellationToken cancellationToken)
        {
            var response = new ResponseDto();

            if (!memoryCache.TryGetValue(request.BatchId, out BulkUploadBatch batch))
            {
                response.IsSuccess = false;
                response.Message = "Upload session expired or invalid batch ID. Please re-upload the file.";
            }

            if (batch?.ValidContacts == null || !batch.ValidContacts.Any())
            {
                response.IsSuccess = false;
                response.Message = "No valid contacts to upload.";
            }
            else
            {
                // Map ContactDto to Contact entities
                var contactEntities = batch.ValidContacts.Select(dto => new Contact
                {
                    TenantId = dto.TenantId,
                    FirstName = dto.FirstName,
                    MiddleName = dto.MiddleName,
                    LastName = dto.LastName,
                    DisplayName = dto.DisplayName,
                    PhoneNumber = dto.PhoneNumber,
                    Email = dto.Email,
                    Company = dto.Company,
                    JobTitle = dto.JobTitle,
                    Timezone = dto.Timezone,
                    CountryId = dto.CountryId,
                    LanguageId = dto.LanguageId,
                    ContactSource = dto.ContactSource,
                    ChannelPreference = dto.ChannelPreference,
                    ImportSource = Enum.TryParse<ImportSource>(dto.ImportSource, out var result) ? result : ImportSource.ExcelImport,
                }).ToList();

                var result = await contactRepository.BulkContactSaveInDatabaseAsync(contactEntities, cancellationToken);

                memoryCache.Remove(request.BatchId);

                response.IsSuccess = true;
                response.Message = $"{batch.ValidContacts.Count} contacts uploaded successfully.";
            }

            return response;
        }
    }
}
