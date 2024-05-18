using Microsoft.AspNetCore.Components;

namespace HeadlessBlazor.Behaviors;

public interface IReferenceable
{
    ElementReference ElementReference { get; }
}