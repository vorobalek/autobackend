using AutoBackend.Sdk.Extensions;
using Namotion.Reflection;
using NSwag.Generation.Processors;
using NSwag.Generation.Processors.Contexts;

namespace AutoBackend.Sdk.NSwag;

internal sealed class NSwagOperationTagsProcessor : OperationTagsProcessor
{
    protected override void AddControllerNameTag(OperationProcessorContext context)
    {
        if (!context.ControllerType.IsGenericControllerV1())
        {
            base.AddControllerNameTag(context);
            return;
        }

        var genericType = context.ControllerType.GenericTypeArguments[0];
        var controllerTag = genericType.Name;
        var summary = context.ControllerType.GetXmlDocsSummary(new XmlDocsOptions
        {
            ResolveExternalXmlDocs = context.Settings.SchemaSettings.ResolveExternalXmlDocumentation
        });
        context.OperationDescription.Operation.Tags.Add(controllerTag);
        UpdateDocumentTagDescription(context, controllerTag, summary);
    }
}