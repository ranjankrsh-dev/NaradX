using DocumentFormat.OpenXml.VariantTypes;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using NaradX.Domain.Entities.ManageContact;
using NaradX.Domain.Repositories.Interfaces;
using NaradX.Business.Dto.BulkUpload;
using NaradX.Business.Dto.Common;
using NaradX.Business.Dto.Contact;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaradX.Business.Contacts.Commands.BulkUploadContact
{
    public class BulkUploadContactCommandHandler : IRequestHandler<BulkUploadContactCommand, ResponseDto>
    {
        private readonly IMemoryCache memoryCache;
        private readonly ILogger<BulkUploadContactCommandHandler> logger;
        private readonly IContactRepository contactRepository;
        private readonly AutoMapper.IMapper mapper;

        public BulkUploadContactCommandHandler(IMemoryCache memoryCache, ILogger<BulkUploadContactCommandHandler> logger, IContactRepository contactRepository, AutoMapper.IMapper mapper)
        {
            this.memoryCache = memoryCache;
            this.logger = logger;
            this.contactRepository = contactRepository;
            this.mapper = mapper;
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
                var validContacts = mapper.Map<List<Contact>>(batch.ValidContacts);
                validContacts.ForEach(c => {
                    c.TenantId = 1;
                    c.IsActive = true;
                    c.CreatedOn = DateTime.UtcNow;
                });
                var result = await contactRepository.BulkContactSaveInDatabase(validContacts, cancellationToken);

                memoryCache.Remove(request.BatchId);

                response.IsSuccess = true;
                response.Message = $"{batch.ValidContacts.Count} contacts uploaded successfully.";
            }

            return response;
        }
    }
}
