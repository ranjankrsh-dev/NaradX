using DocumentFormat.OpenXml.VariantTypes;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using NaradX.Domain.Entities.ManageContact;
using NaradX.Domain.Repositories.Interfaces;
using NaradX.Shared.Dto.BulkUpload;
using NaradX.Shared.Dto.Common;
using NaradX.Shared.Dto.Contact;
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
                var result = await contactRepository.BulkContactSaveInDatabase(batch.ValidContacts, cancellationToken);

                memoryCache.Remove(request.BatchId);

                response.IsSuccess = true;
                response.Message = $"{batch.ValidContacts.Count} contacts uploaded successfully.";
            }

            return response;
        }
    }
}
