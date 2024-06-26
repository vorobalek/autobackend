using System.Reflection;
using AutoBackend.Sdk.Attributes;
using AutoBackend.Sdk.Builders;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace AutoBackend.Sdk.Controllers.Infrastructure;

internal sealed class GenericControllerTypeFeatureProvider(params Assembly[] assemblies)
    : IApplicationFeatureProvider<ControllerFeature>
{
    public void PopulateFeature(IEnumerable<ApplicationPart> parts, ControllerFeature feature)
    {
        foreach (var currentAssembly in assemblies)
        {
            var candidates = currentAssembly.GetExportedTypes();
            foreach (var candidate in candidates
                         .Where(candidate => candidate
                             .GetCustomAttribute<GenericControllerAttribute>() is not null))
                feature.Controllers.Add(GenericControllerTypeBuilder.BuildForCandidate(candidate).GetTypeInfo());
        }
    }
}