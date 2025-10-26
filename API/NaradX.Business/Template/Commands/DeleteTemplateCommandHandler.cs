using MediatR;
using NaradX.Domain.Repositories.Interfaces;

namespace NaradX.Business.Template.Commands;

public class DeleteTemplateCommandHandler(ITemplateRepository templateRepository) : IRequestHandler<DeleteTemplateCommand, bool>
{
    public async Task<bool> Handle(DeleteTemplateCommand request, CancellationToken cancellationToken)
    {
        return await templateRepository.DeleteWhatsAppMessageTemplateByNameAsync(request.Name, cancellationToken);
    }
}
