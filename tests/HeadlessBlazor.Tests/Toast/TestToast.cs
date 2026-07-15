using Microsoft.AspNetCore.Components;

namespace HeadlessBlazor.Tests.Toast;

/// <summary>
/// Minimal component used as a toast body in service-level tests. <see cref="ToastService"/> only
/// stores its <see cref="System.Type"/> and parameters (it never instantiates the component itself),
/// so an empty body is enough to exercise the service.
/// </summary>
public sealed class TestToast : ComponentBase
{
    [Parameter] public string? Message { get; set; }
}
