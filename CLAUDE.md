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

Each component family is its own NuGet package (project under `src/`). All packages are versioned together in lockstep — the version is derived from the git tag by MinVer (configured in `src/Directory.Build.props`), so no `.csproj` carries a `<Version>` element and releasing is just `git tag v1.0.1-preview.1 && git push --tags`. Prerelease numbers must be dot-separated (`-preview.11`, not `-preview11`) — only an all-digit identifier compares numerically under SemVer, so `1.0.0-preview10` sorts *below* `1.0.0-preview2` on nuget.org. Versions `1.0.0-preview2` through `1.0.0-preview10` are published with that flaw; `1.0.0-preview9` is the highest-sorting of them, which is why the line moved to `1.0.1-preview.*`.

- `HeadlessBlazor.Core` — shared base classes (`HBElement`, `HBElementBase`), no component-specific logic. Everything else depends on this.
- `HeadlessBlazor.Dropdown`, `HeadlessBlazor.Modal`, `HeadlessBlazor.FloatingElement`, `HeadlessBlazor.OutsideClick` — individually installable component packages.
- `HeadlessBlazor` (assembly name `HeadlessBlazor.Bundle`) — meta-package that references all of the above for a single-install experience. `AddHeadlessBlazor()` in `ServiceCollectionExtensions.cs` registers each package's own DI extension (currently just `AddHeadlessBlazorModal()`).

`docs/HeadlessBlazor.Docs` (server, WASM host) + `docs/HeadlessBlazor.Docs.Client` (WASM client, holds the actual example `.razor` files under `Examples/`) together form the documentation site at headlessblazor.org, built with `AddInteractiveWebAssemblyComponents()`.

## Component architecture

Components are plain C# classes (not `.razor` files) that build their render tree manually via `RenderTreeBuilder`. This lets every element-emitting component share one rendering pipeline instead of duplicating markup per component.

**Base classes** (`HeadlessBlazor.Core`):
- `HBElementBase` — renders a single configurable HTML element (`ElementName`, default `"div"`). Passes through unmatched attributes via `UserAttributes`, and supports `OnClickStopPropagation`/`OnClickPreventDefault`. Subclasses hook into rendering via `OnBeforeOpenElement` / `OnBeforeCloseElement` / `OnAfterCloseElement`, all of which take `ref int sequence` so derived classes can splice content into the same render tree rather than nesting `BuildRenderTree` calls. `OnInitialized` is `sealed`; use `OnBeforeInitialized`/`OnAfterInitialized` instead.
- `HBElement` / `HBElement<T>` — adds `ChildContent` (plain `RenderFragment`, or `RenderFragment<T>` when child content needs to reference the component itself, e.g. for compound components).

**Compound component pattern** (see Dropdown): a root component (`HBDropdown`) owns open/closed state and overrides `BuildRenderTree` (not just the `OnBefore*` hooks) to wrap its own render tree in a `CascadingValue<TSelf>`. Child parts (`HBDropdownMenu`, `HBDropdownTrigger`, etc.) receive the root via `[CascadingParameter]` and read/mutate state through it. The root exposes `OpenAsync`/`CloseAsync`/`ToggleAsync`, plus `OnOpen`/`OnClose` callbacks, all funneled through `InvokeAsync` for thread-safety. This only works when the trigger and content live together in the same markup call site — Modal deliberately does not use this pattern (see below).

**Service-driven pattern** (Modal): `HBModal`/`HBModalOverlay`/`HBModalContent` do not exist — instead `IModalService.ShowAsync<TComponent>(...)` opens any component as a modal body from anywhere (a click handler, another service), independent of where in the tree the caller lives, and can resolve a typed `ModalResult<T>`. `ModalService` (internal, registered via `AddHeadlessBlazorModal()`) holds the list of currently-open `ModalInstance`s and raises `StateChanged`; `HBModalHost` subscribes to that event and renders one `HBModalHostItem` per instance directly (no wrapper element, no JS relocation). `HBModalHost` must be mounted once, directly in the host page (e.g. `App.razor`) as a sibling of `<Routes>`/`<HeadOutlet>` — **not** inside `MainLayout` or anything else nested under `<Routes>` — so its element is a genuine child of `<body>` from the moment Blazor inserts it (mirroring how `HeadOutlet` itself is mounted directly in `<head>`). An earlier version rendered `HBModalHost` anywhere (e.g. `MainLayout`) and portaled it to the end of `<body>` via JS `appendChild` (`HBPortalBehavior`); that relocation desynced Blazor's internal parent tracking from the real DOM and could crash a later re-render with `Cannot read properties of null (reading 'removeChild')` — see the docs site's `App.razor`/`MainLayout.razor` for the corrected placement. Each `HBModalHostItem` renders the overlay/dialog `<div>`s as siblings (not nested — so a click inside the dialog can't bubble into the overlay's close handler), applies `ModalOptions.OverlayAttributes`/`ContentAttributes`, mounts `HBFocusTrapBehavior` on the dialog element, and cascades `IModalInstance` (so the body component can call `CloseAsync(result)`/`CancelAsync()`) and `IModalContentRegistrar` (so `HBModalTitle`/`HBModalDescription` can still register `aria-labelledby`/`aria-describedby`, unchanged from before) into a `<DynamicComponent Type="Instance.ComponentType" Parameters="Instance.Parameters">`. `HBModalTrigger<TComponent>` is sugar around injecting `IModalService` and calling `ShowAsync` on click. `HBModalClose` now cascades off `IModalInstance` and calls `CancelAsync()`.

`HeadlessBlazor.Toast` still uses the older JS-portal pattern (`HBToastHost` + its own copy of `HBPortalBehavior`) and is susceptible to the same desync — apply the same host-page-mounting fix there if it surfaces.

The dialog `<div>` has the native `autofocus` attribute rather than relying on `HBFocusTrapBehavior`'s JS to make the first move — `HBFocusTrapBehavior`'s `createInstance` call still has to round-trip through JS interop (dynamic import + construct), and until that resolves, focus (and therefore the `@onkeydown` Escape handler, which only fires for elements inside the currently-focused subtree) hasn't moved into the dialog. `autofocus` is a browser-native mechanism applied synchronously as the element is inserted, so Escape works and the dialog already has focus with zero interop gap; `HBFocusTrapBehavior` then refines that a moment later into "focus the first focusable descendant" plus Tab-cycling. `HBFocusTrapBehavior` also guards against a related race: if a modal is closed again before its `import`/`createInstance` chain resolves, it checks a `disposed` flag before touching `Element` (which may already be removed from the DOM by then) and before handing off a JS handle nothing would ever dispose.

**JS interop pattern** (`HBFloatBehavior`, `HBPortalBehavior`, `HBOutsideClickBehavior`, focus trap): implemented as `.razor` files (not plain C# classes) with a matching `.razor.js` file colocated next to it and served from `./_content/{PackageName}/{ComponentName}.razor.js`. Each component:
  1. Implements `IAsyncDisposable`, disposing the JS module reference in `DisposeAsync`.
  2. In `OnAfterRenderAsync(firstRender)`, dynamically imports its JS module and calls a `createInstance` factory, keeping the returned `IJSObjectReference` handle for later calls (`updateOptions`, `dispose`, etc.).
  3. If .NET needs to be called back from JS (e.g. outside-click detection), passes a `DotNetObjectReference<TSelf>` into `createInstance` and exposes a `[JSInvokable]` method.

These interop behaviors are composed into higher-level components rather than used directly — e.g. `HBDropdownMenu` conditionally opens an `HBFloatBehavior` component inside its own render tree when `AutoPosition` is true, using the dropdown's captured `ElementReference` as the anchor.

**Element reference capture**: components needing a JS handle to their rendered element capture it via `builder.AddElementReferenceCapture(sequence, callback)` inside `OnBeforeCloseElement`, storing it in a public `ElementReference` property and calling `StateHasChanged` once populated — other components (e.g. `HBFloatBehavior`'s `Anchor`/`Content` parameters) consume that reference via cascading parameters. In plain `.razor` files (which can't hook `OnBeforeCloseElement`), the equivalent is `@ref="_element"` plus an `OnAfterRender(firstRender) { if (firstRender) StateHasChanged(); }` override — the ref isn't populated on the first render either way, so something must force a second render before a child behavior component (`HBPortalBehavior`, `HBFocusTrapBehavior`) sees a non-empty `Element.Id`. See `HBModalHostItem` (for `HBFocusTrapBehavior`) or `HBToastHost` (for `HBPortalBehavior`).
