# HeadlessBlazor.Modal

A headless modal / dialog component for Blazor. Modals are opened imperatively via
`IModalService`, from anywhere in your app - a button click, another service, a
navigation event - and render any component you author as the modal body. No CSS/markup
opinions: you supply classes and styles, HeadlessBlazor.Modal supplies state, focus
trapping, and accessibility semantics.

## Installation

For the full `HeadlessBlazor` package:
```cmd
dotnet add package HeadlessBlazor
```

For just the modal component:

```cmd
dotnet add package HeadlessBlazor.Modal
```

Add the namespace to `_Imports.razor`:

```razor
@using HeadlessBlazor
```

And register the service:

```csharp
// If using the full HeadlessBlazor package:
builder.Services.AddHeadlessBlazor();

// If using just the modal component:
builder.Services.AddHeadlessBlazorModal();
```

Add an `HBModalHost` to your `MainLayout.razor` (or somewhere global):

```razor
@* MainLayout.razor *@
<HBModalHost />
@Body
```

## Usage

Author your modal's body as an ordinary component:

```razor
@* ConfirmDialog.razor *@
<HBModalTitle>Confirm action</HBModalTitle>
<HBModalDescription>Are you sure you want to continue?</HBModalDescription>
<div>
    <HBModalClose>Cancel</HBModalClose>
    <button @onclick="Confirm">Confirm</button>
</div>

@code {
    [CascadingParameter]
    public IModalInstance? Modal { get; set; }

    private Task Confirm() => Modal!.CloseAsync(true);
}
```

Then show it from anywhere with `IModalService`:

```razor
@inject IModalService ModalService

<button @onclick="DeleteAsync">Delete</button>

@code {
    private async Task DeleteAsync()
    {
        var result = await ModalService.ShowAsync<ConfirmDialog, bool>();

        if (!result.Canceled && result.Data)
        {
            // deleted
        }
    }
}
```

Or use `HBModalTrigger<TComponent>` as sugar for the common "open this on click" case:

```razor
<HBModalTrigger TComponent="ConfirmDialog">Delete</HBModalTrigger>
```

## Options

Behavior and attribute passthrough are controlled by `ModalOptions`:

| Property | Default | Description |
| --- | --- | --- |
| `CloseOnEscape` | `true` | Cancel the modal when the escape key is pressed. |
| `CloseOnOutsideClick` | `true` | Cancel the modal when the overlay (backdrop) is clicked. |
| `OverlayAttributes` | `null` | Attributes (e.g. `class`, `style`) applied to the overlay element. |
| `ContentAttributes` | `null` | Attributes (e.g. `class`, `style`) applied to the dialog content element. |
| `TransitionDuration` | `null` | When set, enables enter/exit transitions (see [Transitions](#transitions)). |

Since HeadlessBlazor.Modal ships no styling of its own, `OverlayAttributes`/`ContentAttributes`
are how you position and style the backdrop and dialog.

### Per-modal options

Pass a `ModalOptions` at the call site to configure a single modal:

```csharp
var options = new ModalOptions
{
    CloseOnOutsideClick = false,
    ContentAttributes = new Dictionary<string, object?>
    {
        ["class"] = "dialog"
    }
};

await ModalService.ShowAsync<ConfirmDialog>(options);
```

`HBModalTrigger<TComponent>` accepts the same object via its `Options` parameter:

```razor
<HBModalTrigger TComponent="ConfirmDialog" Options="_options">Delete</HBModalTrigger>
```

### Global default options

Most apps want one consistent overlay/dialog look everywhere. Configure defaults once at
registration and every modal opened without its own options inherits them:

```csharp
// Full HeadlessBlazor package:
builder.Services.AddHeadlessBlazor(configureModalDefaults: options =>
{
    options.OverlayAttributes = new Dictionary<string, object?>
    {
        ["style"] = "position:fixed;inset:0;background:rgba(0,0,0,.5);"
    };
    options.ContentAttributes = new Dictionary<string, object?>
    {
        ["class"] = "modal-dialog"
    };
});

// Or just the modal component:
builder.Services.AddHeadlessBlazorModal(configureDefaults: options => { /* ... */ });
```

With defaults configured, call sites can omit options entirely:

```csharp
await ModalService.ShowAsync<ConfirmDialog>();
```

> **Note:** per-call options *replace* the global defaults rather than merging with them.
> A `ModalOptions` passed to `ShowAsync`/`HBModalTrigger` is used verbatim, so it must
> re-specify any overlay/dialog styling you still want. (Merging is intentionally avoided:
> the `bool` options can't distinguish "explicitly set to `false`" from "left unset", so a
> merge could silently clobber an intentional `false`.)

## Transitions

By default a modal appears and disappears instantly. Set `TransitionDuration` to animate it in
and out. When set, the overlay and dialog elements each get a `data-state` attribute that you
transition off of in CSS:

- On open, `data-state` starts at `"closed"` and flips to `"open"` on the next frame, so a CSS
  transition plays the modal in.
- On close, `data-state` returns to `"closed"` and the modal stays mounted for
  `TransitionDuration` so the exit transition can play before the element is removed.

Set `TransitionDuration` to match your CSS transition duration:

```csharp
builder.Services.AddHeadlessBlazorModal(configureDefaults: options =>
{
    options.TransitionDuration = TimeSpan.FromMilliseconds(200);
    options.OverlayAttributes = new Dictionary<string, object?> { ["class"] = "modal-overlay" };
    options.ContentAttributes = new Dictionary<string, object?> { ["class"] = "modal-dialog" };
});
```

```css
.modal-overlay {
    opacity: 0;
    transition: opacity .2s ease;
}
.modal-overlay[data-state="open"] {
    opacity: 1;
}

.modal-dialog {
    opacity: 0;
    transform: scale(.96);
    transition: opacity .2s ease, transform .2s ease;
}
.modal-dialog[data-state="open"] {
    opacity: 1;
    transform: scale(1);
}
```

The library owns the timing (mounting, the enter flip, and holding the element through the exit),
but the animation itself is entirely yours - use any properties and easing you like. No
`data-state` attribute is rendered at all when `TransitionDuration` is `null`.

## Results

`ModalResult` (and the strongly-typed `ModalResult<TResult>` returned by
`ShowAsync<TComponent, TResult>()`) distinguishes `Canceled` from `Data`: a modal
dismissed via `CancelAsync()`, the escape key, `HBModalClose`, or an outside click resolves
with `Canceled = true`, which is not the same as `Modal.CloseAsync(false)` resolving with
`Canceled = false, Data = false`. This matters for confirm dialogs in particular - you can
tell "the user explicitly said no" apart from "the user dismissed the dialog without
deciding."

## Stacking

Opening a modal while another is already open works - `HBModalHost` renders every open
instance, oldest first, so later (newer) modals paint on top by default DOM stacking.
Escape/outside-click/focus-trap are scoped per instance, so they only affect whichever
modal currently has focus.
