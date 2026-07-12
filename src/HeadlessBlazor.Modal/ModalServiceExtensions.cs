using Microsoft.AspNetCore.Components;

namespace HeadlessBlazor;

/// <summary>
/// Strongly-typed-result convenience overloads for <see cref="IModalService"/>.
/// </summary>
public static class ModalServiceExtensions
{
    /// <summary>
    /// Shows <typeparamref name="TComponent"/> as a modal, casting its result to <typeparamref name="TResult"/>.
    /// </summary>
    public static async Task<ModalResult<TResult>> ShowAsync<TComponent, TResult>(this IModalService modalService, ModalOptions? options = null)
        where TComponent : IComponent
    {
        var result = await modalService.ShowAsync<TComponent>(options);
        return ToTypedResult<TResult>(result);
    }

    /// <summary>
    /// Shows <typeparamref name="TComponent"/> as a modal with parameters bound to it, casting its result to <typeparamref name="TResult"/>.
    /// </summary>
    public static async Task<ModalResult<TResult>> ShowAsync<TComponent, TResult>(this IModalService modalService, IDictionary<string, object?> parameters, ModalOptions? options = null)
        where TComponent : IComponent
    {
        var result = await modalService.ShowAsync<TComponent>(parameters, options);
        return ToTypedResult<TResult>(result);
    }

    private static ModalResult<TResult> ToTypedResult<TResult>(ModalResult result)
    {
        return result.Cancelled
            ? ModalResult<TResult>.Cancel()
            : ModalResult<TResult>.Ok((TResult)result.Data!);
    }
}
