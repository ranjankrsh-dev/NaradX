using MediatR;
using NaradX.Business.Dto.Template;
using NaradX.Business.Models;

namespace NaradX.Business.Template.Commands;

public class CreateTemplateCommand(WhatsAppMessageTemplateDTO whatsAppTemplate) : IRequest<CreateTemplateResponse>
{
    public WhatsAppMessageTemplateDTO WhatsAppTemplate { get; set; } = whatsAppTemplate;
}
