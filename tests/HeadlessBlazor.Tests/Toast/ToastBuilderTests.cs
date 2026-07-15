namespace HeadlessBlazor.Tests.Toast;

public class ToastBuilderTests
{
    private static ToastService CreateService() => new(new ToastOptions { Duration = null });

    [Fact]
    public void WithParam_BindsParameterByExpression()
    {
        var service = CreateService();

        _ = service.Create<TestToast>().WithParam(x => x.Message, "Hello").Show();

        Assert.Equal("Hello", service.Instances[0].Parameters["Message"]);
    }

    [Fact]
    public void Show_CopiesParameters_SoLaterReuseDoesNotMutateShownToast()
    {
        var service = CreateService();
        var builder = service.Create<TestToast>().WithParam(x => x.Message, "First");

        _ = builder.Show();
        var shownToastParameters = service.Instances[0].Parameters;

        // Reconfiguring the same builder must not write into the already-shown toast's dictionary.
        builder.WithParam(x => x.Message, "Second");

        Assert.Equal("First", shownToastParameters["Message"]);
    }

    [Fact]
    public void WithParam_LastValueWins()
    {
        var service = CreateService();

        _ = service.Create<TestToast>()
            .WithParam(x => x.Message, "First")
            .WithParam(x => x.Message, "Last")
            .Show();

        Assert.Equal("Last", service.Instances[0].Parameters["Message"]);
    }
}
