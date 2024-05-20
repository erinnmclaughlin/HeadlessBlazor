using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace HeadlessBlazor;

public class HBElement : HBElementBase
{
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    protected override void AddChildContent(ref int sequence, RenderTreeBuilder builder)
    {
        if (ChildContent != null)
            builder.AddContent(sequence, ChildContent);
    }
}

public class HBElement<T> : HBElementBase where T : HBElement<T>
{
    [Parameter]
    public RenderFragment<T>? ChildContent { get; set; }

    protected override void AddChildContent(ref int sequence, RenderTreeBuilder builder)
    {
        if (ChildContent != null)
            builder.AddContent(sequence, ChildContent, (T)this);
    }
}
