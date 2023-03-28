using AutoBackend.Sdk.Attributes;

namespace Api.Data;

[GenericEntity]
[GenericController]
[GenericGqlQuery]
public class Note
{
    public string Content { get; set; } = null!;
}