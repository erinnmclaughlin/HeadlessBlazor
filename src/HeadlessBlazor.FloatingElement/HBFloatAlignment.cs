using System.Text.Json.Serialization;

namespace HeadlessBlazor;

/// <summary>
/// The alignment of a floating element.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum HBFloatAlignment
{
    /// <summary>
    /// Align the floating element to the start of the anchor element.
    /// </summary>
    Start,

    /// <summary>
    /// Align the floating element to the center of the anchor element.
    /// </summary>
    Center,

    /// <summary>
    /// Align the floating element to the end of the anchor element.
    /// </summary>
    End
}
