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

// TODO: make this work!
//public abstract class HBElement<T> : HBElementBase
//{
//    protected abstract T? Content { get; }

//    [Parameter]
//    public RenderFragment<T?>? ChildContent { get; set; }

//    protected override void AddChildContent(RenderTreeBuilder builder, int sequence)
//    {
//        builder.AddContent(sequence, ChildContent?.Invoke(Content));
//    }
//}
