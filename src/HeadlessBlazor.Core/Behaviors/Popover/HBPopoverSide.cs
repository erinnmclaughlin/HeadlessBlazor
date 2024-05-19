using System.Text.Json.Serialization;

namespace HeadlessBlazor;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum HBPopoverSide
{
    Top,
    Right,
    Bottom,
    Left
}
