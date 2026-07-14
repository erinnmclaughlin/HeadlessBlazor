using System.Linq.Expressions;

namespace HeadlessBlazor;

/// <summary>
/// Binds a modal's parameters and then shows it via <see cref="ShowAsync"/>. Obtained from
/// <see cref="IModalService.Create{TComponent, TResult}"/>. Nothing is shown until
/// <see cref="ShowAsync"/> is called, so a builder that is never awaited never opens a modal.
/// </summary>
/// <typeparam name="TComponent">The component to render as the modal's body.</typeparam>
/// <typeparam name="TResult">The type of value the modal resolves with, as declared by <typeparamref name="TComponent"/>.</typeparam>
public sealed class ModalBuilder<TComponent, TResult> where TComponent : IModalComponent<TResult>
{
    private readonly IModalService _modalService;
    private readonly Dictionary<string, object?> _parameters = [];

    /// <summary>
    /// Creates a builder that shows its modal through <paramref name="modalService"/>. Prefer
    /// <see cref="IModalService.Create{TComponent, TResult}"/> to constructing this directly; it is
    /// public only so that an <see cref="IModalService"/> implementation (such as a test double) can
    /// return one.
    /// </summary>
    /// <param name="modalService">The service the configured modal is shown through.</param>
    /// <param name="defaultOptions">
    /// The app-wide modal option defaults.
    /// </param>
    public ModalBuilder(IModalService modalService)
    {
        ArgumentNullException.ThrowIfNull(modalService);

        _modalService = modalService;
    }

    /// <summary>
    /// Binds <paramref name="value"/> to the <typeparamref name="TComponent"/> parameter selected by
    /// <paramref name="parameter"/>, e.g. <c>.WithParam(x => x.Title, "My Title")</c>. Selecting the
    /// parameter by expression rather than by name means both the name and the value's type are
    /// checked by the compiler. Calling it more than once for the same parameter keeps the last value.
    /// </summary>
    /// <typeparam name="TValue">The type of the selected parameter.</typeparam>
    /// <param name="parameter">Selects the parameter to bind, as a direct property access on <typeparamref name="TComponent"/>.</param>
    /// <param name="value">The value to bind to it.</param>
    /// <returns>The same builder, so calls can be chained.</returns>
    /// <exception cref="ArgumentException">
    /// <paramref name="parameter"/> does not select a <see cref="ParameterAttribute"/>-marked property
    /// directly on <typeparamref name="TComponent"/>.
    /// </exception>
    public ModalBuilder<TComponent, TResult> WithParam<TValue>(Expression<Func<TComponent, TValue>> parameter, TValue value)
    {
        ArgumentNullException.ThrowIfNull(parameter);

        _parameters[HBExpressionUtils.GetComponentParameterName(parameter)] = value;
        return this;
    }

    /// <summary>
    /// Shows the configured modal.
    /// </summary>
    /// <param name="options">
    /// The options for this modal. Falls back to the app-wide default options.
    /// </param>
    /// <returns>
    /// A task that completes with the modal's <see cref="ModalResult{TResult}"/> once it is closed or canceled.
    /// </returns>
    public Task<ModalResult<TResult>> ShowAsync(ModalOptions? options = null)
    {
        // Hand over a copy: the shown modal holds onto its parameter dictionary for as long as it is
        // open, so a builder that is reused (or configured further) must not write into it.
        return _modalService.ShowAsync<TComponent, TResult>(new Dictionary<string, object?>(_parameters), options);
    }
}
