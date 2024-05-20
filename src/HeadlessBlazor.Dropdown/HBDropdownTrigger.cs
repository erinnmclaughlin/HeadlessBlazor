using Microsoft.AspNetCore.Components;

namespace HeadlessBlazor;

/// <summary>
/// The dropdown trigger.
/// </summary>
public class HBDropdownTrigger : HBElement
{
    /// <summary>
    /// The parent <see cref="HBDropdown"/> component.
    /// </summary>
    [CascadingParameter]
    public HBDropdown Dropdown { get; set; } = default!;

    /// <inheritdoc />
    [Parameter]
    public override string ElementName { get; set; } = "button";

    /// <inheritdoc />
    protected override void OnParametersSet()
    {
        if (!OnClickPreventDefault)
            UserAttributes.TryAdd("onclick", new EventCallback(this, Dropdown.ToggleAsync));
    }
}
