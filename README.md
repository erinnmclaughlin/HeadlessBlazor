[![Headless Blazor Header Image](assets/HeadlessBlazorHeader.svg)](https://headlessblazor.org)

[Headless Blazor](https://headlessblazor.org) is a style-agnostic component library for Blazor. Inspired by [Headless UI](https://headlessui.com).

## Installation

> [!NOTE]  
> This library is very much a work-in-progress. Contributions are welcome!

### Quick Start

```cmd
dotnet add package HeadlessBlazor
```

#### Looking for individual components?
Each component is packaged separately, so if you're only after one or two specific components, you can install those individually:

```cmd
dotnet add package HeadlessBlazor.{Component}
```

## HeadlessBlazor Packages

| Package | Latest Release | Description |
| :-- | :-- | :-- |
| [HeadlessBlazor](src/HeadlessBlazor) | [![NuGet Version](https://img.shields.io/nuget/vpre/HeadlessBlazor?logo=NuGet)](https://www.nuget.org/packages/HeadlessBlazor) | A bundle package that includes all core HeadlessBlazor components and behaviors. |
| [HeadlessBlazor.Dropdown](src/HeadlessBlazor.Dropdown) | [![NuGet Version](https://img.shields.io/nuget/vpre/HeadlessBlazor.Dropdown?logo=NuGet)](https://www.nuget.org/packages/HeadlessBlazor.Dropdown) | A headless dropdown component. |
| [HeadlessBlazor.FloatingElement](src/HeadlessBlazor.FloatingElement) | [![NuGet Version](https://img.shields.io/nuget/vpre/HeadlessBlazor.FloatingElement?logo=NuGet)](https://www.nuget.org/packages/HeadlessBlazor.FloatingElement) | Enables dynamic float behavior for components like popovers, tooltips, dropdowns, etc. |
| [HeadlessBlazor.OutsideClick](src/HeadlessBlazor.OutsideClick) | [![NuGet Version](https://img.shields.io/nuget/vpre/HeadlessBlazor.OutsideClick?logo=NuGet)](https://www.nuget.org/packages/HeadlessBlazor.OutsideClick) | Enables handling of click events that occur outside of a component. |
| HeadlessBlazor.Modal | TBA | A headless modal component. |
