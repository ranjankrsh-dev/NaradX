// <copyright file="GetTemplateByNameQuery.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace NaradX.Business.Template.Query;

using MediatR;
using NaradX.Business.Dtos.Template;

public class GetTemplateByNameQuery(string name) : IRequest<WhatsAppMessageTemplateDTO>
{
    public string Name { get; } = name;
}
