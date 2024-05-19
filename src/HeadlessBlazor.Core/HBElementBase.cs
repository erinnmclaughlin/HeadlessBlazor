﻿using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;

namespace HeadlessBlazor;

public abstract class HBElementBase : ComponentBase
{
    public virtual string ElementName { get; set; } = "div";

    [Parameter(CaptureUnmatchedValues = true)]
    public virtual IDictionary<string, object?> UserAttributes { get; set; } = new Dictionary<string, object?>();

    [Parameter]
    public bool OnClickStopPropagation { get; set; }

    [Parameter]
    public bool OnClickPreventDefault { get; set; }

    protected virtual void OnBeforeInitialized() { }
    protected virtual void OnAfterInitialized() { }
    protected sealed override void OnInitialized()
    {
        OnBeforeInitialized();

        if (UserAttributes.ContainsKey("__internal_preventDefault_onclick"))
            OnClickPreventDefault = true;

        if (UserAttributes.ContainsKey("__internal_stopPropagation_onclick"))
            OnClickStopPropagation = true;

        OnAfterInitialized();
    }

    protected void BuildRenderTree(RenderTreeBuilder builder, ref int sequenceNumber)
    {
        var createElement = !string.IsNullOrWhiteSpace(ElementName);

        if (createElement)
        {
            builder.OpenElement(sequenceNumber++, ElementName);

            foreach (var attr in UserAttributes)
            {
                if (attr.Value != null)
                    builder.AddAttribute(sequenceNumber, attr.Key, attr.Value);
            }

            builder.AddAttribute(sequenceNumber, "data-hb-tag", GetType().Name);

            AddEventHandlers(builder, ref sequenceNumber);
            builder.AddEventStopPropagationAttribute(sequenceNumber, "onclick", OnClickStopPropagation);
            builder.AddEventPreventDefaultAttribute(sequenceNumber, "onclick", OnClickPreventDefault);

            AddElementReference(builder, ref sequenceNumber);
        }

        AddBehaviors(builder, ref sequenceNumber);

        AddChildContent(builder, ref sequenceNumber);

        if (createElement)
            builder.CloseElement();
    }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        var seq = 0;
        BuildRenderTree(builder, ref seq);
    }

    protected virtual void AddChildContent(RenderTreeBuilder builder, ref int sequence)
    {
    }

    protected virtual void AddBehaviors(RenderTreeBuilder builder, ref int sequenceNumber)
    {
    }

    protected virtual void AddElementReference(RenderTreeBuilder builder, ref int sequenceNumber)
    {
    }

    protected virtual void AddEventHandlers(RenderTreeBuilder builder, ref int sequenceNumber)
    {
    }
}
