using MediatR;
using NaradX.Business.Dto.Template;

namespace NaradX.Business.Template.Query;

public record GetAllTemplateQuery : IRequest<List<WhatsAppMessageTemplateDTO>>
{
}
