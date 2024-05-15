namespace BlazorWASM.Pages;

public partial class DropdownExamples
{
    private const string Example1CodeBlock = """
        <Dropdown class="dropdown d-inline-block">
            <DropdownTrigger class="btn btn-primary dropdown-toggle">Click Me</DropdownTrigger>
            <DropdownItems class="dropdown-menu show">
                <DropdownItem class="dropdown-item">Default</DropdownItem>
                <DropdownItem class="dropdown-item" type="button" ElementName="button">Button</DropdownItem>
                <DropdownItem class="dropdown-item" type="button" disabled="true" ElementName="button">Disabled Button</DropdownItem>
                <DropdownItem class="dropdown-item" type="button" ElementName="button" OnClickStopPropagation="true">Button (Stop Propagation)</DropdownItem>
                <DropdownItem class="dropdown-item" href="#" ElementName="a">Link</DropdownItem>
                <DropdownItem class="dropdown-item" href="#" ElementName="a" OnClickPreventDefault="true">Link (Prevent Default)</DropdownItem>
                <DropdownItem class="dropdown-item" href="#" ElementName="a" OnClickPreventDefault="true" OnClickStopPropagation="true">Link (Prevent Default and Stop Propagation)</DropdownItem>
            </DropdownItems>
        </Dropdown>
        """;
}