# HeadlessBlazor.Toast

A headless toast / snackbar component for Blazor. Toasts are shown imperatively via
`IToastService`, from anywhere in your app - a button click, a save handler, another
service - and render any component you author as the toast body. No CSS/markup opinions:
you supply classes and styles, HeadlessBlazor.Toast supplies state, stacking, and
auto-dismiss timing.

## Installation

For the full `HeadlessBlazor` package:
```cmd
dotnet add package HeadlessBlazor
```

For just the toast component:

```cmd
dotnet add package HeadlessBlazor.Toast
```

Add the namespace to `_Imports.razor`:

```razor
@using HeadlessBlazor
```

And register the service:

```csharp
// If using the full HeadlessBlazor package:
builder.Services.AddHeadlessBlazor();

// If using just the toast component:
builder.Services.AddHeadlessBlazorToast();
```

Add an `HBToastHost` to your `MainLayout.razor` (or somewhere global):

```razor
@* MainLayout.razor *@
<HBToastHost />
@Body
```

## Usage

Author your toast's body as an ordinary component:

```razor
@* SuccessToast.razor *@
@inherits HBToast

<p>@Message</p>
<HBToastClose>Dismiss</HBToastClose>

@code {
    [Parameter, EditorRequired]
    public required string Message { get; set; }
}
```

Then show it from anywhere with `IToastService`:

```razor
@inject IToastService ToastService

<button @onclick="Save">Save</button>

@code {
    private void Save()
    {
        // ... save ...

        ToastService.Create<SuccessToast>()
            .WithParam(x => x.Message, "Saved successfully!")
            .Show();
    }
}
```

`Create<TComponent>()` starts a fluent chain that binds parameters by expression and shows the toast
on `Show()`, which returns the `IToastInstance` handle. Because each parameter is selected with an
expression rather than a string key, the compiler checks both the name and the value's type - a
renamed parameter breaks the build instead of silently binding nothing. Selecting a property that
isn't marked `[Parameter]` throws right at the `WithParam` call.

Nothing is shown until `Show()` is called. When the parameters are only known at runtime,
`Show<TComponent>(parameters)` still takes a dictionary directly:

```csharp
ToastService.Show<SuccessToast>(new Dictionary<string, object?>
{
    [nameof(SuccessToast.Message)] = message
});
```

Or use `HBToastTrigger<TComponent>` as sugar for the common "show this on click" case:

```razor
<HBToastTrigger TComponent="SuccessToast" Parameters="_params">Save</HBToastTrigger>
```

## Options

Behavior and attribute passthrough are controlled by `ToastOptions`:

| Property | Default | Description |
| --- | --- | --- |
| `Duration` | 5 seconds | How long the toast stays visible before auto-dismissing. Set to `null` to require manual dismissal. |
| `Attributes` | `null` | Attributes (e.g. `class`, `style`) applied to the toast element. |
| `TransitionDuration` | `null` | When set, enables enter/exit transitions (see [Transitions](#transitions)). |

Since HeadlessBlazor.Toast ships no styling of its own, `Attributes` is how you style each toast.

### Per-toast options

Pass a `ToastOptions` at the call site to configure a single toast. It replaces the global defaults
outright rather than merging with them, so re-specify any styling you still want:

```csharp
ToastService.Create<SuccessToast>()
    .WithParam(x => x.Message, "Saved successfully!")
    .Show(new ToastOptions { Duration = TimeSpan.FromSeconds(10) });
```

The same object works on the non-builder overloads:

```csharp
var options = new ToastOptions
{
    Duration = TimeSpan.FromSeconds(10),
    Attributes = new Dictionary<string, object?> { ["class"] = "toast toast-success" }
};

ToastService.Show<SuccessToast>(options);
```

`HBToastTrigger<TComponent>` accepts the same object via its `Options` parameter.

### Global default options

Configure defaults once at registration and every toast shown without its own options
inherits them:

```csharp
// Full HeadlessBlazor package:
builder.Services.AddHeadlessBlazor(configureToastDefaults: options =>
{
    options.Attributes = new Dictionary<string, object?> { ["class"] = "toast" };
});

// Or just the toast component:
builder.Services.AddHeadlessBlazorToast(configureDefaults: options => { /* ... */ });
```

> **Note:** per-call options *replace* the global defaults rather than merging with them,
> the same as `ModalOptions` in HeadlessBlazor.Modal.

## Positioning and stacking

`HBToastHost` renders every shown toast, oldest first, inside a single container element that
is portaled to the end of `<body>`. Pass attributes directly to `HBToastHost` to position and
stack that container (e.g. fixed to a corner of the viewport):

```razor
<HBToastHost class="hb-toast-stack" />
```

```css
.hb-toast-stack {
    position: fixed;
    bottom: 1rem;
    right: 1rem;
    display: flex;
    flex-direction: column;
    gap: .5rem;
}
```

## Transitions

By default a toast appears and disappears instantly. Set `TransitionDuration` to animate it in
and out - the same mechanism as `ModalOptions.TransitionDuration` in HeadlessBlazor.Modal:

- On show, `data-state` starts at `"closed"` and flips to `"open"` on the next frame, so a CSS
  transition plays the toast in.
- On dismiss, `data-state` returns to `"closed"` and the toast stays mounted for
  `TransitionDuration` so the exit transition can play before the element is removed.

```csharp
builder.Services.AddHeadlessBlazorToast(configureDefaults: options =>
{
    options.TransitionDuration = TimeSpan.FromMilliseconds(200);
    options.Attributes = new Dictionary<string, object?> { ["class"] = "hb-toast" };
});
```

```css
.hb-toast {
    opacity: 0;
    transform: translateY(.5rem);
    transition: opacity .2s ease, transform .2s ease;
}
.hb-toast[data-state="open"] {
    opacity: 1;
    transform: translateY(0);
}
```

The library owns the timing (mounting, the enter flip, holding the element through the exit,
and the auto-dismiss countdown), but the animation itself is entirely yours.
