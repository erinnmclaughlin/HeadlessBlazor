using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;

namespace HeadlessBlazor.Core;

public abstract class HBElementBase : ComponentBase
{
    [Parameter]
    public ElementReference? Element { get; set; }

    [Parameter]
    public EventCallback<ElementReference> ElementChanged { get; set; }

    [Parameter]
    public string HtmlTag { get; set; } = "div";

    [Parameter(CaptureUnmatchedValues = true)]
    public IDictionary<string, object?> UserAttributes { get; set; } = new Dictionary<string, object?>();

    [Parameter]
    public bool StopClickPropagation { get; set; }

    [Parameter]
    public bool PreventClickDefault { get; set; }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        base.BuildRenderTree(builder);

        builder.OpenElement(0, HtmlTag);

        foreach (var attr in UserAttributes)
        {
            if (attr.Value != null)
                builder.AddAttribute(1, attr.Key, attr.Value);
        }

        builder.AddEventStopPropagationAttribute(2, "onclick", !StopClickPropagation);
        builder.AddEventPreventDefaultAttribute(3, "onclick", PreventClickDefault);

        if (Element.HasValue)
        {
            builder.AddElementReferenceCapture(4, async element =>
            {
                Element = element;
                await ElementChanged.InvokeAsync(Element.Value);
            });
        }

        AddChildContent(builder, 5);

        builder.CloseElement();
    }

    protected virtual void AddChildContent(RenderTreeBuilder builder, int sequence)
    {
    }
}

public abstract class HBElement<T> : HBElementBase
{
    protected abstract T? Content { get; }

    [Parameter]
    public RenderFragment<T?>? ChildContent { get; set; }

    protected override void AddChildContent(RenderTreeBuilder builder, int sequence)
    {
        builder.AddContent(sequence, ChildContent?.Invoke(Content));
    }
}

public class HBElement : HBElementBase
{
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    protected override void AddChildContent(RenderTreeBuilder builder, int sequence)
    {
        builder.AddContent(sequence, ChildContent);
    }
}
