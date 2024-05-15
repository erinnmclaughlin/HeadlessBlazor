namespace BlazorWASM.Pages;

public partial class DropdownExamples
{
    private const string Example1CodeBlock = """
        <Dropdown class="dropdown d-inline-block">
            <DropdownTrigger class="btn btn-primary dropdown-toggle">Click Me</DropdownTrigger>
            <DropdownItems class="dropdown-menu show">
                <DropdownItem class="dropdown-item">Default</DropdownItem>
                <DropdownItemButton class="dropdown-item" type="button">Button</DropdownItemButton>
                <DropdownItemButton class="dropdown-item" type="button" disabled="true">Disabled Button</DropdownItemButton>
                <DropdownItemButton class="dropdown-item" type="button"@onclick:stopPropagation="true">Button (Stop Propagation)</DropdownItemButton>
                <DropdownItemLink class="dropdown-item" href="#">Link</DropdownItemLink>
                <DropdownItemLink class="dropdown-item" href="#" @onclick:preventDefault="true">Link (Prevent Default)</DropdownItemLink>
                <DropdownItemLink class="dropdown-item" href="#" @onclick:stopPropagation="true">Link (Stop Propagation)</DropdownItemLink>
                <DropdownItemLink class="dropdown-item" href="#" OnClickPreventDefault="true" OnClickStopPropagation="true">Link (Prevent Default and Stop Propagation)</DropdownItemLink>
            </DropdownItems>
        </Dropdown>
        """;
}