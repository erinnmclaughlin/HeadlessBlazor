using Microsoft.AspNetCore.Components;

namespace HeadlessBlazor;

/// <summary>
/// A button that opens <typeparamref name="TComponent"/> as a modal via <see cref="IModalService"/>
/// when clicked.
/// </summary>
/// <typeparam name="TComponent">The component to render as the modal's body.</typeparam>
public class HBModalTrigger<TComponent> : HBElement
    where TComponent : IComponent
{
    [Inject]
    private IModalService ModalService { get; set; } = default!;

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
    /// Invoked after the modal closes or is cancelled, with its result.
    /// </summary>
    [Parameter]
    public EventCallback<ModalResult> OnClosed { get; set; }

    /// <inheritdoc />
    [Parameter]
    public override string ElementName { get; set; } = "button";

    /// <inheritdoc />
    protected override void OnParametersSet()
    {
        if (!OnClickPreventDefault)
            UserAttributes.TryAdd("onclick", new EventCallback(this, HandleClickAsync));
    }

    private async Task HandleClickAsync()
    {
        var result = Parameters is null
            ? await ModalService.ShowAsync<TComponent>(Options)
            : await ModalService.ShowAsync<TComponent>(Parameters, Options);

        await OnClosed.InvokeAsync(result);
    }
}
