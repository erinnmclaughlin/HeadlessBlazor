using Microsoft.AspNetCore.Components;

namespace HeadlessBlazor.Tests;

/// <summary>
/// Minimal component used as a modal body in service-level tests. <see cref="ModalService"/> only
/// stores its <see cref="System.Type"/> and parameters (it never instantiates the component itself),
/// so an empty body is enough to exercise the service.
/// </summary>
public sealed class TestModal : ComponentBase, IModalComponent<bool>
{
    [Parameter] public string? Title { get; set; }
}
