namespace HeadlessBlazor;

/// <summary>
/// A button that shows <typeparamref name="TComponent"/> as a toast via <see cref="IToastService"/>
/// when clicked.
/// </summary>
/// <typeparam name="TComponent">The component to render as the toast's body.</typeparam>
public class HBToastTrigger<TComponent> : HBElement where TComponent : IComponent
{
    [Inject]
    private IToastService ToastService { get; set; } = null!;

    /// <summary>
    /// Parameter values to bind to <typeparamref name="TComponent"/>, keyed by parameter name.
    /// </summary>
    [Parameter]
    public IDictionary<string, object?>? Parameters { get; set; }

    /// <summary>
    /// Behavioral options for the toast.
    /// </summary>
    [Parameter]
    public ToastOptions? Options { get; set; }

    /// <inheritdoc />
    [Parameter]
    public override string ElementName { get; set; } = "button";

    /// <inheritdoc />
    protected override void OnParametersSet()
    {
        UserAttributes.TryAdd("type", "button");
        UserAttributes.TryAdd("onclick", new EventCallback(this, HandleClick));
    }

    private void HandleClick()
    {
        if (Parameters is null)
            ToastService.Show<TComponent>(Options);
        else
            ToastService.Show<TComponent>(Parameters, Options);
    }
}
