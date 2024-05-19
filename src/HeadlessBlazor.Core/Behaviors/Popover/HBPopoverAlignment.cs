using System.Text.Json.Serialization;

namespace HeadlessBlazor;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum HBPopoverAlignment
{
    Start,
    Center,
    End
}
