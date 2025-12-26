// <copyright file="IWhatsAppApiGateway.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace NaradX.Infrastructure.Gateways.WhatsApp;

using Microsoft.EntityFrameworkCore;
using NaradX.Business.Dtos.Template;
using Refit;

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

    [Post("/v18.0/{phoneNumberId}/messages")]
    Task<ApiResponse<string>> SendMessageAsync(
        [AliasAs("phoneNumberId")] string phoneNumberId,
        [Body] object payload,
        [Header("Authorization")] string authorization);

}
