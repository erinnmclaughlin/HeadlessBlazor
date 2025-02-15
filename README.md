[![Headless Blazor Header Image](assets/HeadlessBlazorHeader.svg)](https://headlessblazor.org)

[Headless Blazor](https://headlessblazor.org) is a style-agnostic component library for Blazor. Inspired by [Headless UI](https://headlessui.com).

## Installation

> [!NOTE]  
> This library is very much a work-in-progress. Contributions are welcome!

### Quick Start

Install the bundle from [NuGet](https://www.nuget.org/packages/HeadlessBlazor):

```cmd
dotnet add package HeadlessBlazor
```

Then add the following to your `Program.cs`:
```csharp
builder.Services.AddHeadlessBlazor();
```

And add a using to your `_Imports.razor` file:
```razor
@using HeadlessBlazor
```

## Individual Packages

> [!TIP]
> **Looking for individual components?**
> Each component is packaged separately, so if you're only after one or two specific components, you can install those individually.

#### HeadlessBlazor.Dropdown
> A headless dropdown component.
>
> [![NuGet Version](https://img.shields.io/nuget/vpre/HeadlessBlazor.Dropdown?logo=NuGet)](https://www.nuget.org/packages/HeadlessBlazor.Dropdown)


#### HeadlessBlazor.FloatingElement
> Enables dynamic float behavior for components like popovers, tooltips, dropdowns, etc.
>
> [![NuGet Version](https://img.shields.io/nuget/vpre/HeadlessBlazor.FloatingElement?logo=NuGet)](https://www.nuget.org/packages/HeadlessBlazor.FloatingElement)


#### HeadlessBlazor.OutsideClick
> Enables handling of click events that occur outside of a component.
>
> [![NuGet Version](https://img.shields.io/nuget/vpre/HeadlessBlazor.OutsideClick?logo=NuGet)](https://www.nuget.org/packages/HeadlessBlazor.OutsideClick)
