namespace HeadlessBlazor;

/// <summary>
/// Represents an open modal. Cascaded to the component rendered as the modal's body, so it
/// can close or cancel itself.
/// </summary>
public interface IModalInstance
{
    /// <summary>
    /// A unique identifier for this modal instance.
    /// </summary>
    Guid Id { get; }

    /// <summary>
    /// The options this modal was shown with.
    /// </summary>
    ModalOptions Options { get; }

    /// <summary>
    /// Closes the modal, resolving the <see cref="IModalService.ShowAsync{TComponent}(ModalOptions?)"/>
    /// call that opened it with <see cref="ModalResult.Ok"/>.
    /// </summary>
    /// <param name="result">The value to resolve the modal's result with, if any.</param>
    Task CloseAsync(object? result = null);

    /// <summary>
    /// Cancels the modal, resolving the <see cref="IModalService.ShowAsync{TComponent}(ModalOptions?)"/>
    /// call that opened it with <see cref="ModalResult.Cancel"/>.
    /// </summary>
    Task CancelAsync();
}
