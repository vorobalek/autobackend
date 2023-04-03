using System.Reflection;
using AutoBackend.Sdk.Attributes;
using AutoBackend.Sdk.Helpers;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace AutoBackend.Sdk.Controllers.Infrastructure;

internal sealed class GenericControllerTypeFeatureProvider : IApplicationFeatureProvider<ControllerFeature>
{
    private readonly Assembly[] _assemblies;

    public GenericControllerTypeFeatureProvider(params Assembly[] assemblies)
    {
        _assemblies = assemblies;
    }

    public void PopulateFeature(IEnumerable<ApplicationPart> parts, ControllerFeature feature)
    {
        foreach (var currentAssembly in _assemblies)
        {
            var candidates = currentAssembly.GetExportedTypes();
            foreach (var candidate in candidates
                         .Where(candidate => candidate
                             .GetCustomAttribute<GenericControllerAttribute>() is not null))
                feature.Controllers.Add(GenericControllerTypeBuilder.BuildForCandidate(candidate).GetTypeInfo());
        }
    }
}