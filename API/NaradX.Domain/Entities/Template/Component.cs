using NaradX.Domain.Entities.Base;
using System.Text.Json.Serialization;

namespace NaradX.Domain.Entities.Template;

public class Component : BaseEntity<Guid>
{
    public string Type { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
    public Example? Example { get; set; }

    //public List<Button>? Buttons { get; set; }
}


