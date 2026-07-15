using Microsoft.AspNetCore.Components;

namespace HeadlessBlazor.Tests.Toast;

/// <summary>
/// A minimal <see cref="IToastInstance"/> test double for asserting that components cascaded
/// under a toast body (e.g. <see cref="HBToastClose"/>) call back into it correctly.
/// </summary>
internal sealed class FakeToastInstance : IToastInstance
{
    public Guid Id { get; } = Guid.NewGuid();

    public ToastOptions Options { get; } = new();

    public int DismissCallCount { get; private set; }

    public Task DismissAsync()
    {
        DismissCallCount++;
        return Task.CompletedTask;
    }
}
