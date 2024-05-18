using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace HeadlessBlazor.Behaviors;

public sealed partial class HBOutsideClickBehavior : ComponentBase, IAsyncDisposable
{
    private IJSObjectReference? jsRef;
    private IJSObjectReference? jsObjRef;
    private DotNetObjectReference<HBOutsideClickBehavior>? objRef;

    [Inject]
    private IJSRuntime JS { get; set; } = default!;

    [Parameter]
    public ICloseable Closeable { get; set; } = default!;

    public async ValueTask DisposeAsync()
    {
        if (jsObjRef is not null)
        {
            await jsObjRef.InvokeVoidAsync("dispose");
            await jsObjRef.DisposeAsync();
        }

        if (jsRef is not null)
            await jsRef.DisposeAsync();

        objRef?.Dispose();
    }

    [JSInvokable]
    public async void NotifyClickOutside()
    {
        await HandleClick();
    }

    protected override void OnInitialized()
    {
        objRef = DotNetObjectReference.Create(this);
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            jsRef = await JS.InvokeAsync<IJSObjectReference>("import", "./_content/HeadlessBlazor.Core/Behaviors/HBOutsideClickBehavior.razor.js");
            jsObjRef = await jsRef.InvokeAsync<IJSObjectReference>("OutsideClickBehavior.createInstance", Closeable.ElementReference, objRef);
        }
    }

    private Task HandleClick() => Closeable is null ? Task.CompletedTask : Closeable.CloseAsync();
}
