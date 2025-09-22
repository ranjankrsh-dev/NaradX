using MediatR;
using NaradX.Business.Template.Commands;
using NaradX.Domain.Entities.Template;
using NaradX.Domain.Repositories;

namespace NaradX.Business.Template.Commands;

public class CreateTemplateCommandHandler(ITemplateRepository templateRepository) : IRequestHandler<CreateTemplateCommand, WhatsAppTemplate>
{
    private readonly ITemplateRepository _templateRepository = templateRepository;

    public async Task<WhatsAppTemplate> Handle(CreateTemplateCommand request, CancellationToken cancellationToken)
    {
        return await _templateRepository.CreateOrderConfirmationTemplateAsync();
    }
}
