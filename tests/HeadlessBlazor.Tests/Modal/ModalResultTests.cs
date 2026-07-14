namespace HeadlessBlazor.Tests.Modal;

public class ModalResultTests
{
    [Fact]
    public void Ok_IsNotCanceled_AndCarriesData()
    {
        var result = ModalResult<int>.Ok(42);

        Assert.False(result.Canceled);
        Assert.Equal(42, result.Data);
    }

    [Fact]
    public void Cancel_IsCanceled_WithDefaultData()
    {
        var result = ModalResult<int>.Cancel();

        Assert.True(result.Canceled);
        Assert.Equal(0, result.Data);
    }

    [Fact]
    public void Ok_WithNullData_IsDistinctFromCancel()
    {
        // The Canceled flag is kept separate from "Data is null" on purpose, so CloseAsync(null)
        // can be told apart from an explicit cancel.
        var closed = ModalResult<string?>.Ok(null);
        var canceled = ModalResult<string?>.Cancel();

        Assert.False(closed.Canceled);
        Assert.True(canceled.Canceled);
    }
}
