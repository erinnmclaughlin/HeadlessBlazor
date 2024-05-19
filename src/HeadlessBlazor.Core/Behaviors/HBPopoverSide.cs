using System.Text.Json.Serialization;

namespace HeadlessBlazor.Behaviors;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum HBPopoverSide
{
    Top,
    Right,
    Bottom,
    Left
}
