using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;

namespace HeadlessBlazor;

/// <summary>
/// A base class for all HeadlessBlazor components.
/// </summary>
public abstract class HBElementBase : ComponentBase
{
    /// <summary>
    /// The type of element to render.
    /// </summary>
    public virtual string ElementName { get; set; } = "div";

    /// <summary>
    /// The HTML attributes to apply to the element.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public virtual IDictionary<string, object?> UserAttributes { get; set; } = new Dictionary<string, object?>();

    /// <summary>
    /// When <see langword="true"/>, will stop propagation of click events. Default is <see langword="false" />.
    /// </summary>
    [Parameter]
    public bool OnClickStopPropagation { get; set; }

    /// <summary>
    /// When <see langword="true"/>, will stop default behavior of click events. Default is <see langword="false" />.
    /// </summary>
    [Parameter]
    public bool OnClickPreventDefault { get; set; }

    /// <inheritdoc />
    protected sealed override void OnInitialized()
    {
        OnBeforeInitialized();

        if (UserAttributes.ContainsKey("__internal_preventDefault_onclick"))
            OnClickPreventDefault = true;

        if (UserAttributes.ContainsKey("__internal_stopPropagation_onclick"))
            OnClickStopPropagation = true;

        OnAfterInitialized();
    }

    /// <summary>
    /// Called before the <see cref="OnInitialized"/> method.
    /// </summary>
    protected virtual void OnBeforeInitialized()
    {
    }

    /// <summary>
    /// Called after the <see cref="OnInitialized"/> method.
    /// </summary>
    protected virtual void OnAfterInitialized() 
    {
    }

    /// <inheritdoc />
    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        var seq = 0;
        BuildRenderTree(ref seq, builder);
    }

    /// <summary>
    /// Renders the component to the supplied <see cref="RenderTreeBuilder"/> starting at the given <paramref name="sequence"/> number
    /// </summary>
    /// <param name="sequence">A reference to the sequence number.</param>
    /// <param name="builder">A <see cref="RenderTreeBuilder"/> that will receive the render output.</param>
    protected void BuildRenderTree(ref int sequence, RenderTreeBuilder builder)
    {
        OnBeforeOpenElement(ref sequence, builder);

        builder.OpenElement(sequence++, ElementName);

        foreach (var attr in UserAttributes)
        {
            if (attr.Value != null)
                builder.AddAttribute(sequence, attr.Key, attr.Value);
        }

        builder.AddEventPreventDefaultAttribute(sequence++, "onclick", OnClickPreventDefault);
        builder.AddEventStopPropagationAttribute(sequence++, "onclick", OnClickStopPropagation);

        OnBeforeCloseElement(ref sequence, builder);

        builder.CloseElement();
    }

    /// <summary>
    /// Adds additional render content to the render tree before the element is opened.
    /// </summary>
    /// <param name="sequence">A reference to the sequence number.</param>
    /// <param name="builder">A <see cref="RenderTreeBuilder"/> that will receive the render output.</param>
    protected virtual void OnBeforeOpenElement(ref int sequence, RenderTreeBuilder builder)
    {
    }

    /// <summary>
    /// Adds additional render content to the render tree before the element is closed.
    /// </summary>
    /// <param name="sequence">A reference to the sequence number.</param>
    /// <param name="builder">A <see cref="RenderTreeBuilder"/> that will receive the render output.</param>
    protected virtual void OnBeforeCloseElement(ref int sequence, RenderTreeBuilder builder)
    {
    }

    /// <summary>
    /// Adds additional render content to the render tree after the element is closed.
    /// </summary>
    /// <param name="sequence">A reference to the sequence number.</param>
    /// <param name="builder">A <see cref="RenderTreeBuilder"/> that will receive the render output.</param>
    protected virtual void OnAfterCloseElement(ref int sequence, RenderTreeBuilder builder)
    {
    }
}
