using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;

namespace HeadlessBlazor;

public abstract class HBElementBase : ComponentBase
{
    public virtual string ElementName { get; set; } = "div";

    [Parameter(CaptureUnmatchedValues = true)]
    public virtual IDictionary<string, object?> UserAttributes { get; set; } = new Dictionary<string, object?>();

    [Parameter]
    public bool OnClickStopPropagation { get; set; }

    [Parameter]
    public bool OnClickPreventDefault { get; set; }

    protected virtual void OnBeforeInitialized() { }
    protected virtual void OnAfterInitialized() { }
    protected sealed override void OnInitialized()
    {
        OnBeforeInitialized();

        if (UserAttributes.ContainsKey("__internal_preventDefault_onclick"))
            OnClickPreventDefault = true;

        if (UserAttributes.ContainsKey("__internal_stopPropagation_onclick"))
            OnClickStopPropagation = true;

        OnAfterInitialized();
    }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        var seq = 0;
        BuildRenderTree(ref seq, builder);
    }

    protected void BuildRenderTree(ref int sequence, RenderTreeBuilder builder)
    {
        var createElement = !string.IsNullOrWhiteSpace(ElementName);

        if (createElement)
        {
            builder.OpenElement(sequence++, ElementName);

            foreach (var attr in UserAttributes)
            {
                if (attr.Value != null)
                    builder.AddAttribute(sequence, attr.Key, attr.Value);
            }

            AddEventHandlers(ref sequence, builder);
            builder.AddEventStopPropagationAttribute(sequence, "onclick", OnClickStopPropagation);
            builder.AddEventPreventDefaultAttribute(sequence, "onclick", OnClickPreventDefault);

            AddElementReference(ref sequence, builder);
        }

        AddBehaviors(ref sequence, builder);
        AddChildContent(ref sequence, builder);

        if (createElement)
            builder.CloseElement();
    }

    protected virtual void AddChildContent(ref int sequence, RenderTreeBuilder builder)
    {
    }

    protected virtual void AddBehaviors(ref int sequence, RenderTreeBuilder builder)
    {
    }

    protected virtual void AddElementReference(ref int sequence, RenderTreeBuilder builder)
    {
    }

    protected virtual void AddEventHandlers(ref int sequence, RenderTreeBuilder builder)
    {
    }
}
