using AutoBackend.Sdk.Controllers.Generic;
using Namotion.Reflection;
using NSwag.Generation.Processors;
using NSwag.Generation.Processors.Contexts;

namespace AutoBackend.Sdk.NSwag;

internal sealed class NSwagOperationTagsProcessor : OperationTagsProcessor
{
    protected override void AddControllerNameTag(OperationProcessorContext context)
    {
        if (!GenericController.IsGenericController(context.ControllerType))
        {
            base.AddControllerNameTag(context);
            return;
        }

        var entityType = GenericController.GetTargetType(context.ControllerType);
        var controllerTag = entityType.Name;
        var summary = context.ControllerType.GetXmlDocsSummary(new XmlDocsOptions
        {
            ResolveExternalXmlDocs = context.Settings.ResolveExternalXmlDocumentation
        });
        context.OperationDescription.Operation.Tags.Add(controllerTag);
        UpdateDocumentTagDescription(context, controllerTag, summary);
    }
}