namespace HeadlessBlazor;

/// <summary>
/// A dropdown item that renders as an HTML button element.
/// </summary>
public class HBDropdownItemButton : HBDropdownItem
{
    /// <inheritdoc />
    public override string ElementName { get; set; } = "button";
}
