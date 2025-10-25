using NaradX.Domain.Entities.Base;
using System.Text.Json.Serialization;

namespace NaradX.Domain.Entities.Template;

public class WhatsAppTemplateDTO
{
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Language { get; set; } = string.Empty;
    public string ParameterFormat { get; set; } = string.Empty;
    public List<Component> Components { get; set; } = [];
}