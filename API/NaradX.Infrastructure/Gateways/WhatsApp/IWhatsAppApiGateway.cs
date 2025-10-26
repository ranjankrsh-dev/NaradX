using Microsoft.EntityFrameworkCore;
using NaradX.Shared.Dto.Template;
using NaradX.Shared.Models;
using Refit;

namespace NaradX.Infrastructure.Gateways.WhatsApp;

public interface IWhatsAppApiGateway
{
    [Headers("Content-Type: application/json")]
    [Post("/v18.0/{businessId}/message_templates")]
    Task<CreateTemplateResponse> CreateTemplateAsync(
        [AliasAs("businessId")] string businessId,
        [Body] WhatsAppMessageTemplateDTO request,
        [Header("Authorization")] string authorization,
        [Header("Content-Type")] string contentType = "application/json"
    );

    [Delete("/v18.0/{businessId}/message_templates")]
    Task<ApiResponse<string>> DeleteTemplateAsync(
        [AliasAs("businessId")] string businessId,
        [Query] string name,
        [Header("Authorization")] string authorization
    );
}
