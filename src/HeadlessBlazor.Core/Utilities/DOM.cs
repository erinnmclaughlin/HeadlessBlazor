using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace HeadlessBlazor.Utilities;

public sealed class DOM(IJSRuntime js) : IAsyncDisposable
{
    private readonly Lazy<ValueTask<IJSObjectReference>> _module = new(() => js.InvokeAsync<IJSObjectReference>("import", "./_content/HeadlessBlazor.Core/utils.js"));

    public async ValueTask<WindowDimensions> GetWindowDimensionsAsync()
    {
        return await InvokeAsync<WindowDimensions>("getWindowDimensions");
    }

    public async ValueTask<ClientRect> GetBoundingClientRect(ElementReference element)
    {
        return await InvokeAsync<ClientRect>("getBoundingClientRect", element);
    }

    public async ValueTask DisposeAsync()
    {
        if (_module.IsValueCreated)
        {
            var module = await _module.Value;
            await module.DisposeAsync();
        }
    }

    private async ValueTask<T> InvokeAsync<T>(string identifier, params object?[]? args)
    {
        var module = await _module.Value;
        return await module.InvokeAsync<T>(identifier, args);
    }
}
