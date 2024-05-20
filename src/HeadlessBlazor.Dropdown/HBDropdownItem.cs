using Microsoft.AspNetCore.Components;

namespace HeadlessBlazor;

/// <summary>
/// A dropdown item.
/// </summary>
public class HBDropdownItem : HBElement
{
    /// <summary>
    /// The parent <see cref="HBDropdown"/> component.
    /// </summary>
    [CascadingParameter]
    public HBDropdown Dropdown { get; set; } = default!;

    /// <inheritdoc />
    protected override void OnParametersSet()
    {
        UserAttributes.TryAdd("onclick", new EventCallback(this, HandleClick));
    }

    /// <summary>
    /// The click event handler.
    /// </summary>
    protected virtual async Task HandleClick()
    {
        if (!OnClickStopPropagation)
            await Dropdown.OnClickItem.InvokeAsync(this);
    }
}
