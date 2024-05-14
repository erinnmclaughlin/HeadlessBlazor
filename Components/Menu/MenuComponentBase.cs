using Microsoft.AspNetCore.Components;

namespace HeadlessBlazor;

public abstract class MenuComponentBase : ComponentBase
{
    public string? Id => UserAttributes?.TryGetValue("id", out var id) is true ? id?.ToString() : null;

    [Parameter(CaptureUnmatchedValues = true)]
    public IDictionary<string, object?>? UserAttributes { get; set; }

    [CascadingParameter]
    public Menu Menu { get; set; } = default!;

    protected override sealed void OnParametersSet()
    {
        if (Menu == null)
        {
            throw new InvalidOperationException($"{nameof(MenuItem)} requires a cascading parameter of type {typeof(Menu).Name}.");
        }

        OnAfterParametersSet();
    }

    protected virtual void OnAfterParametersSet()
    {

    }
}
