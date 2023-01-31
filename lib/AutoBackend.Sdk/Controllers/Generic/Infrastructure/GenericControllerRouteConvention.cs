using System.Reflection;
using AutoBackend.Sdk.Attributes.GenericControllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace AutoBackend.Sdk.Controllers.Generic.Infrastructure;

internal sealed class GenericControllerRouteConvention : IControllerModelConvention
{
    public void Apply(ControllerModel controller)
    {
        if (!GenericController.IsGenericController(controller.ControllerType)) return;

        var genericType = GenericController.GetTargetType(controller.ControllerType);
        var genericControllerAttributes = genericType.GetCustomAttributes<GenericControllerAttribute>().ToArray();
        foreach (var genericControllerAttribute in genericControllerAttributes)
        {
            var controllerTypeGenericDefinition = controller.ControllerType.GetGenericTypeDefinition();
            if (!genericControllerAttribute
                    .TargetGenericControllerTypes
                    .Contains(controllerTypeGenericDefinition)) continue;

            var route = genericType.Name.ToLowerInvariant();
            controller.Selectors.Add(new SelectorModel
            {
                AttributeRouteModel =
                    new AttributeRouteModel(new RouteAttribute($"api/{genericControllerAttribute.Version}/{route}"))
            });
            controller.ApiExplorer.GroupName = genericControllerAttribute.Version;
        }
    }
}