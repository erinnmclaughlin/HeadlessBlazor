﻿using HeadlessBlazor.Core.Behaviors;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web.Virtualization;

namespace HeadlessBlazor;

public class DropdownOutsideClickBehavior : HBOutsideClickBehavior
{
    [CascadingParameter]
    public Dropdown Dropdown { get; set; } = default!;

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        if (Dropdown.IsOpen)
            base.BuildRenderTree(builder);
    }

    protected override void OnParametersSet()
    {
        if (Dropdown == null)
        {
            throw new InvalidOperationException($"{GetType().Name} requires a cascading parameter of type {typeof(Dropdown).Name}.");
        }

        UserAttributes.TryAdd("onclick", new EventCallback(this, HandleClick));

        base.OnParametersSet();
    }

    private async Task HandleClick()
    {
        await Dropdown.Close();
    }
}
