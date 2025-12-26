// <copyright file="GetAllTemplateQuery.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace NaradX.Business.Template.Query;

using MediatR;
using NaradX.Business.Dtos.Template;

public record GetAllTemplateQuery : IRequest<List<WhatsAppMessageTemplateDTO>>
{
}
