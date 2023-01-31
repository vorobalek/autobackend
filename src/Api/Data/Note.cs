using AutoBackend.Sdk.Attributes.GenericControllers.V1;
using AutoBackend.Sdk.Attributes.GenericEntities;

namespace Api.Data;

[GenericEntity]
[GenericControllerV1]
public class Note
{
    public string Content { get; set; } = null!;
}