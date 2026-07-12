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

## Results

`ModalResult` (and the strongly-typed `ModalResult<TResult>` returned by
`ShowAsync<TComponent, TResult>()`) distinguishes `Cancelled` from `Data`: a modal
dismissed via `CancelAsync()`, the escape key, `HBModalClose`, or an outside click resolves
with `Cancelled = true`, which is not the same as `Modal.CloseAsync(false)` resolving with
`Cancelled = false, Data = false`. This matters for confirm dialogs in particular - you can
tell "the user explicitly said no" apart from "the user dismissed the dialog without
deciding."

## Stacking

Opening a modal while another is already open works - `HBModalHost` renders every open
instance, oldest first, so later (newer) modals paint on top by default DOM stacking.
Escape/outside-click/focus-trap are scoped per instance, so they only affect whichever
modal currently has focus.
