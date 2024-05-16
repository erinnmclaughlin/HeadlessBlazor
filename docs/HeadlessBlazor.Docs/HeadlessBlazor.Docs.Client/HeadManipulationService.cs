using Microsoft.JSInterop;

namespace HeadlessBlazor.Docs.Client;

public class HeadManipulationService(IJSRuntime jsRuntime)
{
    private readonly IJSRuntime _jsRuntime = jsRuntime;

    public ValueTask SetTheme(string theme)
    {
        return _jsRuntime.InvokeVoidAsync("headManipulation.setTheme", theme);
    }

    public ValueTask AddLinkAsync(string href, string rel)
    {
        return _jsRuntime.InvokeVoidAsync("headManipulation.addLink", href, rel);
    }

    public ValueTask RemoveLinkAsync(string href)
    {
        return _jsRuntime.InvokeVoidAsync("headManipulation.removeLink", href);
    }

    public ValueTask AddScriptAsync(string src)
    {
        return _jsRuntime.InvokeVoidAsync("headManipulation.addScript", src);
    }

    public ValueTask RemoveScriptAsync(string src)
    {
        return _jsRuntime.InvokeVoidAsync("headManipulation.removeScript", src);
    }

    public ValueTask RemoveStyleAsync()
    {
        return _jsRuntime.InvokeVoidAsync("headManipulation.removeStyle");
    }
}
