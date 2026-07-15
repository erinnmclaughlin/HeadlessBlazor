namespace HeadlessBlazor.Tests.Toast;

/// <summary>
/// Layer 2 (rendered component) tests for <see cref="HBToastTrigger{TComponent}"/>. No JS interop
/// is involved (showing a toast is plain .NET via the injected <see cref="IToastService"/>), so
/// these exercise the full render + click behavior via bUnit against a fake service.
/// </summary>
public class HBToastTriggerTests : BunitContext
{
    [Fact]
    public void RendersButton_WithTypeButton()
    {
        Services.AddSingleton<IToastService>(new FakeToastService());

        var cut = Render<HBToastTrigger<TestToast>>();

        Assert.Equal("button", cut.Find("button").GetAttribute("type"));
    }

    [Fact]
    public void Click_ShowsTheToast()
    {
        var service = new FakeToastService();
        Services.AddSingleton<IToastService>(service);

        var cut = Render<HBToastTrigger<TestToast>>();
        cut.Find("button").Click();

        Assert.Equal(1, service.ShowCallCount);
    }

    [Fact]
    public void Click_PassesConfiguredParameters()
    {
        var service = new FakeToastService();
        Services.AddSingleton<IToastService>(service);
        var parameters = new Dictionary<string, object?> { ["Message"] = "Saved" };

        var cut = Render<HBToastTrigger<TestToast>>(ps => ps.Add(p => p.Parameters, parameters));
        cut.Find("button").Click();

        Assert.Same(parameters, service.LastParameters);
    }
}
