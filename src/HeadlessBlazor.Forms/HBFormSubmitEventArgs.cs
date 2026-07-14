namespace HeadlessBlazor;

/// <summary>
/// Passed to <see cref="HBEditForm{TModel}.OnValidSubmit"/> and <see cref="HBEditForm{TModel}.OnInvalidSubmit"/>.
/// </summary>
/// <typeparam name="TModel">The type of the model being submitted.</typeparam>
/// <param name="Model">The model that was submitted.</param>
/// <param name="CancellationToken">
/// <see cref="HBEditForm{TModel}.CancellationTokenSource"/>'s token if the caller supplied one, otherwise
/// <see cref="System.Threading.CancellationToken.None"/>.
/// </param>
public readonly record struct HBFormSubmitEventArgs<TModel>(HBEditContext<TModel> Context, CancellationToken CancellationToken) where TModel : notnull;
