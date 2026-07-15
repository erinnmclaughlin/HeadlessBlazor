[![Headless Blazor Header Image](https://raw.githubusercontent.com/erinnmclaughlin/HeadlessBlazor/refs/heads/main/assets/HeadlessBlazorHeader.svg)](https://www.nuget.org/packages/HeadlessBlazor)

[Headless Blazor](https://www.nuget.org/packages/HeadlessBlazor) is a style-agnostic component library for Blazor. Inspired by [Headless UI](https://headlessui.com).

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


#### [HeadlessBlazor.DarkMode](./src/HeadlessBlazor.DarkMode)
>
> [![NuGet Version](https://img.shields.io/nuget/vpre/HeadlessBlazor.DarkMode?logo=NuGet)](https://www.nuget.org/packages/HeadlessBlazor.DarkMode)


#### [HeadlessBlazor.Dropdown](./src/HeadlessBlazor.Dropdown)
> A headless dropdown component.
>
> [![NuGet Version](https://img.shields.io/nuget/vpre/HeadlessBlazor.Dropdown?logo=NuGet)](https://www.nuget.org/packages/HeadlessBlazor.Dropdown)


#### [HeadlessBlazor.FloatingElement](./src/HeadlessBlazor.FloatingElement)
> Enables dynamic float behavior for components like popovers, tooltips, dropdowns, etc.
>
> [![NuGet Version](https://img.shields.io/nuget/vpre/HeadlessBlazor.FloatingElement?logo=NuGet)](https://www.nuget.org/packages/HeadlessBlazor.FloatingElement)


#### [HeadlessBlazor.Forms](./src/HeadlessBlazor.Forms)
>
> [![NuGet Version](https://img.shields.io/nuget/vpre/HeadlessBlazor.Forms?logo=NuGet)](https://www.nuget.org/packages/HeadlessBlazor.Forms)


#### [HeadlessBlazor.Modal](./src/HeadlessBlazor.Modal)
>
> [![NuGet Version](https://img.shields.io/nuget/vpre/HeadlessBlazor.Modal?logo=NuGet)](https://www.nuget.org/packages/HeadlessBlazor.Modal)


#### [HeadlessBlazor.OutsideClick](./src/HeadlessBlazor.OutsideClick)
> Enables handling of click events that occur outside of a component.
>
> [![NuGet Version](https://img.shields.io/nuget/vpre/HeadlessBlazor.OutsideClick?logo=NuGet)](https://www.nuget.org/packages/HeadlessBlazor.OutsideClick)


#### [HeadlessBlazor.Toast](./src/HeadlessBlazor.Toast)
>
> [![NuGet Version](https://img.shields.io/nuget/vpre/HeadlessBlazor.Toast?logo=NuGet)](https://www.nuget.org/packages/HeadlessBlazor.Toast)
