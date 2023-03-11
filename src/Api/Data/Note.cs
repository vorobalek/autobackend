using AutoBackend.Sdk.Attributes;

namespace Api.Data;

[GenericEntity]
[GenericController]
public class Note
{
    public string Content { get; set; } = null!;
}