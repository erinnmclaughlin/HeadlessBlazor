namespace HeadlessBlazor.Tests.Modal;

public class ModalBuilderTests
{
    [Fact]
    public void WithParam_BindsParameterByExpression()
    {
        var service = new ModalService(new ModalOptions());

        _ = service.Create<TestModal, bool>().WithParam(x => x.Title, "Hello").ShowAsync();

        Assert.Equal("Hello", service.Instances[0].Parameters["Title"]);
    }

    [Fact]
    public void ShowAsync_CopiesParameters_SoLaterReuseDoesNotMutateOpenModal()
    {
        var service = new ModalService(new ModalOptions());
        var builder = service.Create<TestModal, bool>().WithParam(x => x.Title, "First");

        _ = builder.ShowAsync();
        var openModalParameters = service.Instances[0].Parameters;

        // Reconfiguring the same builder must not write into the already-shown modal's dictionary.
        builder.WithParam(x => x.Title, "Second");

        Assert.Equal("First", openModalParameters["Title"]);
    }

    [Fact]
    public void WithParam_LastValueWins()
    {
        var service = new ModalService(new ModalOptions());

        _ = service.Create<TestModal, bool>()
            .WithParam(x => x.Title, "First")
            .WithParam(x => x.Title, "Last")
            .ShowAsync();

        Assert.Equal("Last", service.Instances[0].Parameters["Title"]);
    }
}
