using Microsoft.AspNetCore.Components;

namespace HeadlessBlazor.FloatingUI;

public class FloatingUIService
{
    public async Task<>

    public async Task<IComputedPosition> ComputePositionAsync(ElementReference reference)
    {

    }
}

public interface IPlatform
{

}

public interface IComputePositionConfiguration
{
    
}

public interface IComputedPosition
{

}

public sealed record Placement(Side Side, Alignment? Alignment = null);

public enum Alignment { Start, End }
public enum Axis { X, Y }
public enum Side { Top, Right, Bottom, Left }
public enum Strategy { Absolute, Fixed }

public sealed record Coords(int X, int Y);
public sealed record Dimensions(float Width, float Height);
