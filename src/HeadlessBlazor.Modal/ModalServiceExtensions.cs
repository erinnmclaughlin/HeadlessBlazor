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
        if (result.Cancelled)
            return ModalResult<TResult>.Cancel();

        if (result.Data is null && typeof(TResult).IsValueType && Nullable.GetUnderlyingType(typeof(TResult)) is null)
        {
            throw new InvalidOperationException(
                $"The modal was closed without a result (e.g. by calling CloseAsync() with no argument), but a " +
                $"non-nullable result of type '{typeof(TResult)}' was expected. Pass a value to CloseAsync(result), " +
                $"or change TResult to a nullable type.");
        }

        return ModalResult<TResult>.Ok((TResult)result.Data!);
    }
}
