using MediatR;
using NaradX.Business.Dto.Template;

namespace NaradX.Business.Template.Query;

public class GetTemplateByNameQuery(string name) : IRequest<WhatsAppMessageTemplateDTO>
{
    public string Name { get; } = name;
}
