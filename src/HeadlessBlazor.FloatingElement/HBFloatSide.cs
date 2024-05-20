using System.Text.Json.Serialization;

namespace HeadlessBlazor;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum HBFloatSide
{
    Top,
    Right,
    Bottom,
    Left
}
