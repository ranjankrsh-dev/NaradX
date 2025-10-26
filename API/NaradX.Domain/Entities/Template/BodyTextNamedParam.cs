using NaradX.Domain.Entities.Base;

namespace NaradX.Domain.Entities.Template;

public class BodyTextNamedParam : BaseEntity<int>
{
    public string ParamName { get; set; } = string.Empty;
    public string Example { get; set; } = string.Empty;

}