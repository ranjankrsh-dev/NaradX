using MediatR;
using NaradX.Shared.Dto.Template;
using NaradX.Shared.Models;

namespace NaradX.Business.Template.Commands;

public class CreateTemplateCommand(WhatsAppMessageTemplateDTO whatsAppTemplate) : IRequest<CreateTemplateResponse>
{
    public WhatsAppMessageTemplateDTO WhatsAppTemplate { get; set; } = whatsAppTemplate;
}
