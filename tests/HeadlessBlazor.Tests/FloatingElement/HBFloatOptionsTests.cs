using System.Text.Json;

namespace HeadlessBlazor.Tests.FloatingElement;

/// <summary>
/// Layer 1 (pure logic) tests for <see cref="HBFloatOptions"/> and the
/// <see cref="HBFloatAlignment"/>/<see cref="HBFloatSide"/> enums it carries. These options are
/// serialized to JSON and passed to the JS side (see <c>HBFloatBehavior.razor</c>'s
/// <c>createInstance</c>/<c>updateOptions</c> calls), so the <see cref="System.Text.Json.Serialization.JsonStringEnumConverter"/>
/// on each enum - which is what makes that payload readable by the JS module rather than raw
/// integers - is worth covering directly.
/// </summary>
public class HBFloatOptionsTests
{
    [Fact]
    public void RecordEquality_IsValueBased()
    {
        var a = new HBFloatOptions { Alignment = HBFloatAlignment.Center, Side = HBFloatSide.Bottom };
        var b = new HBFloatOptions { Alignment = HBFloatAlignment.Center, Side = HBFloatSide.Bottom };

        Assert.Equal(a, b);
    }

    [Fact]
    public void RecordEquality_DiffersWhenValuesDiffer()
    {
        var a = new HBFloatOptions { Alignment = HBFloatAlignment.Start };
        var b = new HBFloatOptions { Alignment = HBFloatAlignment.End };

        Assert.NotEqual(a, b);
    }

    [Fact]
    public void Alignment_SerializesAsString()
    {
        var json = JsonSerializer.Serialize(HBFloatAlignment.Center);

        Assert.Equal("\"Center\"", json);
    }

    [Fact]
    public void Side_SerializesAsString()
    {
        var json = JsonSerializer.Serialize(HBFloatSide.Bottom);

        Assert.Equal("\"Bottom\"", json);
    }

    [Fact]
    public void Options_WithUnsetValues_SerializeAsNull()
    {
        var json = JsonSerializer.Serialize(new HBFloatOptions());

        Assert.Equal("""{"Alignment":null,"Side":null}""", json);
    }
}
