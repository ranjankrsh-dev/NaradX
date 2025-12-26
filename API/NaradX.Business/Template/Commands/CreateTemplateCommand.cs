// <copyright file="CreateTemplateCommand.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace NaradX.Business.Template.Commands;

using MediatR;
using NaradX.Business.Dtos.Template;

/// <summary>
/// Command to create a new WhatsApp message template.
/// </summary>
public class CreateTemplateCommand(WhatsAppMessageTemplateDTO whatsAppTemplate) : IRequest<CreateTemplateResponse>
{
    /// <summary>
    /// Gets or sets the WhatsApp message template data.
    /// </summary>
    public WhatsAppMessageTemplateDTO WhatsAppTemplate { get; set; } = whatsAppTemplate;
}
