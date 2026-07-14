using System.Linq.Expressions;

namespace HeadlessBlazor;

/// <summary>
/// Binds a toast's parameters and then shows it via <see cref="Show"/>. Obtained from
/// <see cref="IToastService.Create{TComponent}"/>. Nothing is shown until <see cref="Show"/> is
/// called, so a builder that is dropped never shows a toast.
/// </summary>
/// <typeparam name="TComponent">The component to render as the toast's body.</typeparam>
public sealed class ToastBuilder<TComponent> where TComponent : IComponent
{
    private readonly IToastService _toastService;
    private readonly Dictionary<string, object?> _parameters = [];

    /// <summary>
    /// Creates a builder that shows its toast through <paramref name="toastService"/>. Prefer
    /// <see cref="IToastService.Create{TComponent}"/> to constructing this directly; it is public only
    /// so that an <see cref="IToastService"/> implementation (such as a test double) can return one.
    /// </summary>
    /// <param name="toastService">The service the configured toast is shown through.</param>
    /// <param name="defaultOptions">
    /// The app-wide option defaults for toasts.
    /// </param>
    public ToastBuilder(IToastService toastService)
    {
        ArgumentNullException.ThrowIfNull(toastService);

        _toastService = toastService;
    }

    /// <summary>
    /// Binds <paramref name="value"/> to the <typeparamref name="TComponent"/> parameter selected by
    /// <paramref name="parameter"/>, e.g. <c>.WithParam(x => x.Message, "Saved")</c>. Selecting the
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
    public ToastBuilder<TComponent> WithParam<TValue>(Expression<Func<TComponent, TValue>> parameter, TValue value)
    {
        ArgumentNullException.ThrowIfNull(parameter);

        _parameters[HBExpressionUtils.GetComponentParameterName(parameter)] = value;
        return this;
    }

    /// <summary>
    /// Shows the configured toast.
    /// </summary>
    /// <param name="options">
    /// The options for this toast. Falls back to the app-wide default options.
    /// </param>
    /// <returns>A handle to the shown toast that can be used to dismiss it early.</returns>
    public IToastInstance Show(ToastOptions? options = null)
    {
        // Hand over a copy: the shown toast holds onto its parameter dictionary for as long as it is
        // on screen, so a builder that is reused (or configured further) must not write into it.
        return _toastService.Show<TComponent>(new Dictionary<string, object?>(_parameters), options);
    }
}
