using MediatR;
using NaradX.Shared.Dto.Template;

namespace NaradX.Business.Template.Query;

public record GetAllTemplateQuery : IRequest<List<WhatsAppMessageTemplateDTO>>
{
}
