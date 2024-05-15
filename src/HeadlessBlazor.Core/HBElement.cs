using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace HeadlessBlazor.Core;

public class HBElement : HBElementBase
{
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    protected override void AddChildContent(RenderTreeBuilder builder, ref int sequence)
    {
        builder.AddContent(sequence++, ChildContent);
    }
}

public class HBElement<T> : HBElementBase where T : HBElement<T>
{
    [Parameter]
    public RenderFragment<T>? ChildContent { get; set; }

    protected override void AddChildContent(RenderTreeBuilder builder, ref int sequence)
    {
        builder.AddContent(sequence++, ChildContent, (T)this);
    }
}
