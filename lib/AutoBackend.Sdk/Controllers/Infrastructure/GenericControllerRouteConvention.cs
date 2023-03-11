using AutoBackend.Sdk.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace AutoBackend.Sdk.Controllers.Infrastructure;

internal sealed class GenericControllerRouteConvention : IControllerModelConvention
{
    public void Apply(ControllerModel controller)
    {
        if (!controller.ControllerType.IsGenericControllerV1()) return;

        var genericType = controller.ControllerType.GenericTypeArguments[0];
        var route = genericType.Name.ToLowerInvariant();
        controller.Selectors.Add(new SelectorModel
        {
            AttributeRouteModel =
                new AttributeRouteModel(new RouteAttribute($"api/{GenericController.Version}/{route}"))
        });
        controller.ApiExplorer.GroupName = GenericController.Version;
    }
}