using System.Text.Json.Serialization;

namespace HeadlessBlazor;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum HBFloatAlignment
{
    Start,
    Center,
    End
}
