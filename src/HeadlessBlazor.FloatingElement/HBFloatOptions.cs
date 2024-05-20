namespace HeadlessBlazor;

/// <summary>
/// Options for rendering a floating element.
/// </summary>
public sealed record HBFloatOptions
{
    /// <summary>
    /// The alignment of the floating element, relative to the anchor.
    /// </summary>
    public HBFloatAlignment? Alignment { get; init; }

    /// <summary>
    /// The side of the anchor to render the floating element.
    /// </summary>
    public HBFloatSide? Side { get; init; }
}
