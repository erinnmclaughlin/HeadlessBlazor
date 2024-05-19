using Microsoft.AspNetCore.Components;

namespace HeadlessBlazor;

public interface IReferenceable
{
    ElementReference ElementReference { get; }
}