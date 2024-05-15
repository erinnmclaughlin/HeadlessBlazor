using Microsoft.AspNetCore.Components;

namespace HeadlessBlazor;

public abstract class DropdownComponentBase : ComponentBase
{
    public abstract Type Type { get; }

    [CascadingParameter]
    public Dropdown Dropdown { get; set; } = default!;

    [Parameter(CaptureUnmatchedValues = true)]
    public IDictionary<string, object?>? UserAttributes { get; set; }

    protected override sealed void OnParametersSet()
    {
        if (Dropdown == null)
        {
            throw new InvalidOperationException($"{Type.Name} requires a cascading parameter of type {typeof(Dropdown).Name}.");
        }

        OnAfterParametersSet();
    }

    protected virtual void OnAfterParametersSet() { }
}
