using System.Reflection;
using AutoBackend.Sdk.Attributes.GenericControllers;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace AutoBackend.Sdk.Controllers.Generic.Infrastructure;

internal sealed class GenericTypeControllerFeatureProvider : IApplicationFeatureProvider<ControllerFeature>
{
    private readonly Assembly[] _assemblies;

    public GenericTypeControllerFeatureProvider(params Assembly[] assemblies)
    {
        _assemblies = assemblies;
    }

    public void PopulateFeature(IEnumerable<ApplicationPart> parts, ControllerFeature feature)
    {
        foreach (var currentAssembly in _assemblies)
        {
            var candidates = currentAssembly.GetExportedTypes();
            foreach (var candidate in candidates)
            foreach (var genericControllerAttribute in candidate.GetCustomAttributes<GenericControllerAttribute>())
                feature.Controllers.Add(genericControllerAttribute.MakeControllerTypeForCandidate(candidate)
                    .GetTypeInfo());
        }
    }
}