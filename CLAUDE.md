# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## What this is

Headless Blazor is a style-agnostic Blazor component library (inspired by Headless UI). Components render bare HTML elements with behavior/state wired up, and expose no CSS/markup opinions — consumers supply their own classes via cascading render fragments.

## Build

```cmd
dotnet build HeadlessBlazor.sln
```

There is no test project in the solution — verification is done by running the docs site and exercising components in the browser.

Run the docs/demo site (interactive WebAssembly host with a matching `.Client` project):

```cmd
dotnet run --project docs/HeadlessBlazor.Docs/HeadlessBlazor.Docs.csproj
```

Target framework is `net10.0`. `src/HeadlessBlazor.DarkMode`, `src/HeadlessBlazor.DevicePreferences`, and `src/HeadlessBlazor.ThemeProvider` have leftover `bin`/`obj` output but no `.csproj` and are **not** part of the solution — do not treat them as active projects.

## Package layout

Each component family is its own NuGet package (project under `src/`), independently versioned in its `.csproj`:

- `HeadlessBlazor.Core` — shared base classes (`HBElement`, `HBElementBase`), no component-specific logic. Everything else depends on this.
- `HeadlessBlazor.Dropdown`, `HeadlessBlazor.Modal`, `HeadlessBlazor.FloatingElement`, `HeadlessBlazor.OutsideClick` — individually installable component packages.
- `HeadlessBlazor` (assembly name `HeadlessBlazor.Bundle`) — meta-package that references all of the above for a single-install experience. `AddHeadlessBlazor()` in `ServiceCollectionExtensions.cs` is currently a no-op placeholder for future DI registration.

`docs/HeadlessBlazor.Docs` (server, WASM host) + `docs/HeadlessBlazor.Docs.Client` (WASM client, holds the actual example `.razor` files under `Examples/`) together form the documentation site at headlessblazor.org, built with `AddInteractiveWebAssemblyComponents()`.

## Component architecture

Components are plain C# classes (not `.razor` files) that build their render tree manually via `RenderTreeBuilder`. This lets every element-emitting component share one rendering pipeline instead of duplicating markup per component.

**Base classes** (`HeadlessBlazor.Core`):
- `HBElementBase` — renders a single configurable HTML element (`ElementName`, default `"div"`). Passes through unmatched attributes via `UserAttributes`, and supports `OnClickStopPropagation`/`OnClickPreventDefault`. Subclasses hook into rendering via `OnBeforeOpenElement` / `OnBeforeCloseElement` / `OnAfterCloseElement`, all of which take `ref int sequence` so derived classes can splice content into the same render tree rather than nesting `BuildRenderTree` calls. `OnInitialized` is `sealed`; use `OnBeforeInitialized`/`OnAfterInitialized` instead.
- `HBElement` / `HBElement<T>` — adds `ChildContent` (plain `RenderFragment`, or `RenderFragment<T>` when child content needs to reference the component itself, e.g. for compound components).

**Compound component pattern** (see Dropdown and Modal): a root component (`HBDropdown`, `HBModal`) owns open/closed state and overrides `BuildRenderTree` (not just the `OnBefore*` hooks) to wrap its own render tree in a `CascadingValue<TSelf>`. Child parts (`HBDropdownMenu`, `HBDropdownTrigger`, `HBModalTrigger`, `HBModalOverlay`, `HBModalContent`, etc.) receive the root via `[CascadingParameter]` and read/mutate state through it (e.g. `Modal.ToggleAsync()`). Root components expose `OpenAsync`/`CloseAsync`/`ToggleAsync`, plus `OnOpen`/`OnClose` callbacks, all funneled through `InvokeAsync` for thread-safety.

**JS interop pattern** (`HBFloatBehavior`, `HBPortalBehavior`, `HBOutsideClickBehavior`, focus trap): implemented as `.razor` files (not plain C# classes) with a matching `.razor.js` file colocated next to it and served from `./_content/{PackageName}/{ComponentName}.razor.js`. Each component:
  1. Implements `IAsyncDisposable`, disposing the JS module reference in `DisposeAsync`.
  2. In `OnAfterRenderAsync(firstRender)`, dynamically imports its JS module and calls a `createInstance` factory, keeping the returned `IJSObjectReference` handle for later calls (`updateOptions`, `dispose`, etc.).
  3. If .NET needs to be called back from JS (e.g. outside-click detection), passes a `DotNetObjectReference<TSelf>` into `createInstance` and exposes a `[JSInvokable]` method.

These interop behaviors are composed into higher-level components rather than used directly — e.g. `HBDropdownMenu` conditionally opens an `HBFloatBehavior` component inside its own render tree when `AutoPosition` is true, using the dropdown's captured `ElementReference` as the anchor.

**Element reference capture**: components needing a JS handle to their rendered element capture it via `builder.AddElementReferenceCapture(sequence, callback)` inside `OnBeforeCloseElement`, storing it in a public `ElementReference` property and calling `StateHasChanged` once populated — other components (e.g. `HBFloatBehavior`'s `Anchor`/`Content` parameters) consume that reference via cascading parameters.
