namespace HeadlessBlazor;

/// <summary>
/// A button that dismisses the enclosing toast when clicked (equivalent to calling
/// <see cref="IToastInstance.DismissAsync"/>).
/// </summary>
public class HBToastClose : HBElement
{
    /// <summary>
    /// The enclosing toast.
    /// </summary>
    [CascadingParameter]
    public IToastInstance Toast { get; set; } = null!;

    /// <inheritdoc />
    [Parameter]
    public override string ElementName { get; set; } = "button";

    /// <inheritdoc />
    protected override void OnBeforeInitialized()
    {
        if (Toast is null)
            throw new InvalidOperationException($"{nameof(HBToastClose)} must be used within a toast body rendered by {nameof(HBToastHost)}.");
    }

    /// <inheritdoc />
    protected override void OnParametersSet()
    {
        UserAttributes.TryAdd("type", "button");
        UserAttributes.TryAdd("onclick", new EventCallback(this, Toast.DismissAsync));
    }
}
