namespace HeadlessBlazor;

/// <summary>
/// A button that opens <typeparamref name="TComponent"/> as a modal via <see cref="IModalService"/>
/// when clicked. <typeparamref name="TResult"/> is the result type declared by
/// <typeparamref name="TComponent"/> via <see cref="IModalComponent{TResult}"/>.
/// </summary>
/// <typeparam name="TComponent">The component to render as the modal's body.</typeparam>
/// <typeparam name="TResult">The type of value the modal resolves with.</typeparam>
public class HBModalTrigger<TComponent, TResult> : HBElement where TComponent : IComponent, IModalComponent<TResult>
{
    [Inject]
    private IModalService ModalService { get; set; } = null!;

    /// <summary>
    /// Parameter values to bind to <typeparamref name="TComponent"/>, keyed by parameter name.
    /// </summary>
    [Parameter]
    public IDictionary<string, object?>? Parameters { get; set; }

    /// <summary>
    /// Behavioral options for the modal.
    /// </summary>
    [Parameter]
    public ModalOptions? Options { get; set; }

    /// <summary>
    /// Invoked after the modal closes or is canceled, with its result.
    /// </summary>
    [Parameter]
    public EventCallback<ModalResult<TResult>> OnClosed { get; set; }

    /// <inheritdoc />
    [Parameter]
    public override string ElementName { get; set; } = "button";

    /// <inheritdoc />
    protected override void OnParametersSet()
    {
        UserAttributes.TryAdd("onclick", new EventCallback(this, HandleClickAsync));
    }

    private async Task HandleClickAsync()
    {
        var result = Parameters is null
            ? await ModalService.ShowAsync<TComponent, TResult>(Options)
            : await ModalService.ShowAsync<TComponent, TResult>(Parameters, Options);

        await OnClosed.InvokeAsync(result);
    }
}
