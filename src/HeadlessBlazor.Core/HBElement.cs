using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace HeadlessBlazor;

/// <summary>
/// A base class for implementing a HeadlessBlazor element with child content.
/// </summary>
public class HBElement : HBElementBase
{
    /// <summary>
    /// The child content.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <inheritdoc />
    protected override void OnBeforeCloseElement(ref int sequence, RenderTreeBuilder builder)
    {
        if (ChildContent != null)
            builder.AddContent(sequence, ChildContent);
    }
}

/// <summary>
/// A base class for implementing a HeadlessBlazor element with child content of type <typeparamref name="T"/>.
/// </summary>
public class HBElement<T> : HBElementBase where T : HBElement<T>
{
    /// <summary>
    /// The child content.
    /// </summary>
    [Parameter]
    public RenderFragment<T>? ChildContent { get; set; }

    /// <inheritdoc />
    protected override void OnBeforeCloseElement(ref int sequence, RenderTreeBuilder builder)
    {
        if (ChildContent != null)
            builder.AddContent(sequence, ChildContent, (T)this);
    }
}
