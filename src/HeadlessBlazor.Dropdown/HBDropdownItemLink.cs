namespace HeadlessBlazor;

/// <summary>
/// A dropdown item that renders as an HTML anchor element.
/// </summary>
public class HBDropdownItemLink : HBDropdownItem
{
    /// <inheritdoc />
    public override string ElementName { get; set; } = "a";
}
