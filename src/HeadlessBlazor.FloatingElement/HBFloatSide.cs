using System.Text.Json.Serialization;

namespace HeadlessBlazor;

/// <summary>
/// The side of the anchor to render the floating element.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum HBFloatSide
{
    /// <summary>
    /// Renders the floating element above the anchor.
    /// </summary>
    Top,

    /// <summary>
    /// Renders the floating element to the right of the anchor.
    /// </summary>
    Right,

    /// <summary>
    /// Renders the floating element below the anchor.
    /// </summary>
    Bottom,

    /// <summary>
    /// Renders the floating element to the left of the anchor.
    /// </summary>
    Left
}
