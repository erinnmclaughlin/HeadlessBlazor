using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace HeadlessBlazor.Behaviors;

public abstract class HBBehaviorBase<T> : ComponentBase where T : HBBehaviorBase<T>
{
    [Inject]
    protected IJSRuntime JS { get; set; } = default!;

    protected async ValueTask<IJSObjectReference> LoadModuleAsync()
    {
        var assemblyName = typeof(T).Assembly.GetName().Name!;
        var ns = typeof(T).Namespace!.Replace(assemblyName, "");
        var path = Path.Combine(".", "_content", assemblyName, ns, typeof(T).Name + ".razor.js");

        return await JS.InvokeAsync<IJSObjectReference>("import", path);
    }

    protected async ValueTask<IJSObjectReference> LoadFromModuleAsync(Func<IJSObjectReference, ValueTask<IJSObjectReference>> func)
    {
        var module = await LoadModuleAsync();
        var loadedObject = await func(module);
        await module.DisposeAsync();
        return loadedObject;
    }
}
