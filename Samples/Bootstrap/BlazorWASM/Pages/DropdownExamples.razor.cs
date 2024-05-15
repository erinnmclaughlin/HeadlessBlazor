namespace BlazorWASM.Pages;

public partial class DropdownExamples
{
    private const string Example1CodeBlock = """
        <Dropdown class="dropdown d-inline-block" OnClickItem="item => item.Dropdown.Close()">
            <DropdownTrigger class="btn btn-primary dropdown-toggle">Click Me</DropdownTrigger>
            <DropdownItems class="dropdown-menu show">
                <DropdownItem class="dropdown-item">Option 1</DropdownItem>
                <DropdownItem class="dropdown-item">Option 2</DropdownItem>
                <DropdownItem class="dropdown-item">Option 3</DropdownItem>
            </DropdownItems>
        </Dropdown>
        """;
}