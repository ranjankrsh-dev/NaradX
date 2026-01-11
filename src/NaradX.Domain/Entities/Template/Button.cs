using NaradX.Domain.Entities.Base;

namespace NaradX.Domain.Entities.Template;

public class Button : BaseEntity<Guid>
{
    public string Type { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty;
}