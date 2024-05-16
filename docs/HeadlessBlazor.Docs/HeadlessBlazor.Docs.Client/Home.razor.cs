namespace HeadlessBlazor.Docs.Client;

public partial class Home
{
    private const string Example1CodeBlock = """
        <Dropdown>
            <DropdownOutsideClickBehavior />
            <DropdownTrigger>Click Me</DropdownTrigger>
            <DropdownItems>
                <DropdownItem>Default</DropdownItem>
                <DropdownItemButton>Button</DropdownItemButton>
                <DropdownItemButton disabled="true">Disabled Button</DropdownItemButton>
                <DropdownItemButton @onclick:stopPropagation="true">Button (Stop Propagation)</DropdownItemButton>
                <DropdownItemLink>Link</DropdownItemLink>
                <DropdownItemLink @onclick:preventDefault="true">Link (Prevent Default)</DropdownItemLink>
                <DropdownItemLink @onclick:stopPropagation="true">Link (Stop Propagation)</DropdownItemLink>
                <DropdownItemLink OnClickPreventDefault="true" OnClickStopPropagation="true">Link (Prevent Default and Stop Propagation)</DropdownItemLink>
            </DropdownItems>
        </Dropdown>
        """;
}