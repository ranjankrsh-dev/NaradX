using MediatR;

namespace NaradX.Business.Template.Commands;

public class DeleteTemplateCommand(string name) : IRequest<bool>
{
    public string Name { get; } = name;
}
