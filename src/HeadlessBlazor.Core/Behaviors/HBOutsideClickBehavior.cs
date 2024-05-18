using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace HeadlessBlazor.Behaviors;

public class HBOutsideClickBehavior<T> : HBElement where T : ICloseable
{
    [Parameter, CascadingParameter]
    public T? Closeable { get; set; }

    protected override void OnBeforeInitialized()
    {
        UserAttributes.TryAdd("style", "position: fixed; top:0; right: 0; bottom: 0; left:0;");
    }

    protected override void AddEventHandlers(RenderTreeBuilder builder, ref int sequenceNumber)
    {
        builder.AddAttribute(sequenceNumber++, "onclick", HandleClick);
    }

    private Task HandleClick() => Closeable is null ? Task.CompletedTask : Closeable.CloseAsync();
}
