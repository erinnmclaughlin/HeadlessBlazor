using System.Text.Json.Serialization;

namespace HeadlessBlazor.Behaviors;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum HBPopoverAlignment
{
    Start,
    Center,
    End
}
