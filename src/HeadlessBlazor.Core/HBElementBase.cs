using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;

namespace HeadlessBlazor.Core;

public abstract class HBElementBase : ComponentBase
{
    protected int CurrentSequence { get; set; }

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

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenElement(CurrentSequence++, ElementName);

        if (UserAttributes is { Count: > 0 })
        {
            foreach (var attr in UserAttributes)
            {
                if (attr.Value != null)
                    builder.AddAttribute(CurrentSequence, attr.Key, attr.Value);
            }

            CurrentSequence++;
        }

        builder.AddEventStopPropagationAttribute(CurrentSequence++, "onclick", !StopClickPropagation);
        builder.AddEventPreventDefaultAttribute(CurrentSequence++, "onclick", PreventClickDefault);

        if (Element.HasValue)
        {
            builder.AddElementReferenceCapture(CurrentSequence++, async element =>
            {
                Element = element;
                await ElementChanged.InvokeAsync(Element.Value);
            });
        }

        AddChildContent(builder, CurrentSequence++);

        builder.CloseElement();
    }

    protected virtual void AddChildContent(RenderTreeBuilder builder, int sequence)
    {
    }
}
