# HeadlessBlazor.Modal

A headless modal / dialog component for Blazor. Modals are opened imperatively via
`IModalService`, from anywhere in your app - a button click, another service, a
navigation event - and render any component you author as the modal body. No CSS/markup
opinions: you supply classes and styles, HeadlessBlazor.Modal supplies state, focus
trapping, and accessibility semantics.

## Installation

```cmd
dotnet add package HeadlessBlazor.Modal
```

```razor
@using HeadlessBlazor
```

Register the service:

```csharp
builder.Services.AddHeadlessBlazorModal();
```

> [!IMPORTANT]
> Add exactly one `<HBModalHost />` somewhere in your app - typically once in your main
> layout. It's what actually renders open modals; without it, `ShowAsync` never resolves.

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

        if (!result.Cancelled && result.Data)
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

## How it works

`IModalService.ShowAsync<TComponent>()` doesn't render anything itself - it registers a
`ModalInstance` (component type + parameters + options) with the internal `ModalService`
and returns a `Task<ModalResult>` that resolves once the modal closes. Nothing is
instantiated until `ShowAsync` is called; there's no markup sitting in your page waiting
to be shown.

`HBModalHost` is the one piece of markup you add yourself. It subscribes to the service
and renders one `HBModalHostItem` per open instance, and portals *itself* to the end of
`<body>` (via the same `HBPortalBehavior` used elsewhere in this package) so it doesn't
matter where in your layout you put it.

For each open modal, `HBModalHostItem` renders an overlay `<div>` and a dialog `<div>` as
siblings - a click inside the dialog can never bubble into the overlay's close handler.
The dialog gets `role="dialog"`, `aria-modal="true"`, and the native `autofocus` attribute,
so it already has focus - and its <kbd>Escape</kbd>-to-cancel handler is already live -
the instant it's inserted, with no async gap where <kbd>Escape</kbd> silently does nothing
because focus hasn't moved yet. `HBFocusTrapBehavior` then refines that a moment later
(once its JS interop resolves) into "focus the first focusable descendant" plus full
Tab-cycling, and restores focus to whatever was focused before the modal opened once it
closes. `ModalOptions.OverlayAttributes`/`ContentAttributes` let you apply `class`/`style`/
etc. to the overlay and dialog elements, since there's no markup call site to put them on
directly.

Your body component receives two cascading values:
- **`IModalInstance`** - `CloseAsync(result)` to resolve `ShowAsync` with a value,
  `CancelAsync()` to resolve it as cancelled. `HBModalClose` is a ready-made button that
  calls `CancelAsync()`; a "Confirm"/"Save" button just calls `Modal.CloseAsync(...)`
  directly, as in the example above.
- Internally, an `IModalContentRegistrar` lets `HBModalTitle`/`HBModalDescription` register
  their `id`s so the dialog's `aria-labelledby`/`aria-describedby` stay wired up - you
  don't interact with this directly, just use `HBModalTitle`/`HBModalDescription`.

### Results

`ModalResult` (and the strongly-typed `ModalResult<TResult>` returned by
`ShowAsync<TComponent, TResult>()`) distinguishes `Cancelled` from `Data`: a modal
dismissed via `CancelAsync()`, the escape key, `HBModalClose`, or an outside click resolves
with `Cancelled = true`, which is not the same as `Modal.CloseAsync(false)` resolving with
`Cancelled = false, Data = false`. This matters for confirm dialogs in particular - you can
tell "the user explicitly said no" apart from "the user dismissed the dialog without
deciding."

### Stacking

Opening a modal while another is already open works - `HBModalHost` renders every open
instance, oldest first, so later (newer) modals paint on top by default DOM stacking.
Escape/outside-click/focus-trap are scoped per instance, so they only affect whichever
modal currently has focus.
