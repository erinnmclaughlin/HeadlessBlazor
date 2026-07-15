using Microsoft.AspNetCore.Components;

namespace HeadlessBlazor.Tests.Toast;

/// <summary>
/// A minimal <see cref="IToastService"/> test double for asserting that components which inject
/// <see cref="IToastService"/> (e.g. <see cref="HBToastTrigger{TComponent}"/>) call it correctly,
/// without going through the real service's instance tracking.
/// </summary>
internal sealed class FakeToastService : IToastService
{
    public IDictionary<string, object?>? LastParameters { get; private set; }

    public ToastOptions? LastOptions { get; private set; }

    public int ShowCallCount { get; private set; }

    public ToastBuilder<TComponent> Create<TComponent>() where TComponent : IComponent => new(this);

    public IToastInstance Show<TComponent>(ToastOptions? options = null) where TComponent : IComponent
        => Show<TComponent>(new Dictionary<string, object?>(), options);

    public IToastInstance Show<TComponent>(IDictionary<string, object?> parameters, ToastOptions? options = null) where TComponent : IComponent
    {
        ShowCallCount++;
        LastParameters = parameters;
        LastOptions = options;
        return new FakeToastInstance();
    }
}
