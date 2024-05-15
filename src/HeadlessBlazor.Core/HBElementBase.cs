using HeadlessBlazor.Core.Themes;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;

namespace HeadlessBlazor.Core;

public abstract class HBElementBase : ComponentBase
{
    public virtual string ElementName { get; set; } = "div";

    [Inject]
    private HBTheme? Theme { get; set; }

    [Parameter(CaptureUnmatchedValues = true)]
    public virtual IDictionary<string, object?> UserAttributes { get; set; } = new Dictionary<string, object?>();

    [Parameter]
    public bool OnClickStopPropagation { get; set; }

    [Parameter]
    public bool OnClickPreventDefault { get; set; }

    protected override void OnInitialized()
    {
        Theme?.ApplyDefaults(this);
    }

    protected override void OnParametersSet()
    {
        if (UserAttributes.ContainsKey("__internal_preventDefault_onclick"))
            OnClickPreventDefault = true;

        if (UserAttributes.ContainsKey("__internal_stopPropagation_onclick"))
            OnClickStopPropagation = true;
    }

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
            }

            builder.AddEventStopPropagationAttribute(sequenceNumber, "onclick", OnClickStopPropagation);
            builder.AddEventPreventDefaultAttribute(sequenceNumber, "onclick", OnClickPreventDefault);
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
