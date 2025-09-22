using MediatR;

namespace NaradX.Business.Template.Commands;

public class CreateTemplateCommandHandler : IRequestHandler<CreateTemplateCommand, int>
{
    public Task<int> Handle(CreateTemplateCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
