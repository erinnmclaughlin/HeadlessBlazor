using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;

namespace HeadlessBlazor.Core;

public abstract class HBElementBase : ComponentBase
{
    [Parameter]
    public virtual ElementReference? Element { get; set; }

    [Parameter]
    public virtual EventCallback<ElementReference> ElementChanged { get; set; }

    [Parameter]
    public virtual string ElementName { get; set; } = "div";

    [Parameter]
    public virtual bool PreventClickDefault { get; set; }

    [Parameter]
    public virtual bool StopClickPropagation { get; set; }

    [Parameter(CaptureUnmatchedValues = true)]
    public virtual IDictionary<string, object?> UserAttributes { get; set; } = new Dictionary<string, object?>();

    protected void BuildRenderTree(RenderTreeBuilder builder, ref int sequenceNumber)
    {
        var createElement = !string.IsNullOrWhiteSpace(ElementName);

        if (createElement)
        {
            builder.OpenElement(sequenceNumber++, ElementName);

            if (UserAttributes is { Count: > 0 })
            {
                foreach (var attr in UserAttributes)
                {
                    if (attr.Value != null)
                        builder.AddAttribute(sequenceNumber, attr.Key, attr.Value);
                }

                sequenceNumber++;
            }

            builder.AddEventStopPropagationAttribute(sequenceNumber++, "onclick", !StopClickPropagation);
            builder.AddEventPreventDefaultAttribute(sequenceNumber++, "onclick", PreventClickDefault);

            if (Element.HasValue)
            {
                builder.AddElementReferenceCapture(sequenceNumber++, async element =>
                {
                    Element = element;
                    await ElementChanged.InvokeAsync(Element.Value);
                });
            }
        }

        AddChildContent(builder, ref sequenceNumber);

        if (createElement)
            builder.CloseElement();
    }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        var seq = 0;
        BuildRenderTree(builder, ref seq);
    }

    protected virtual void AddChildContent(RenderTreeBuilder builder, ref int sequence)
    {
    }
}
