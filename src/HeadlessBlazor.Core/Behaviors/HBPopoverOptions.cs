namespace HeadlessBlazor.Behaviors;

public sealed record HBPopoverOptions
{
    public HBPopoverAlignment? Alignment { get; init; }
    public HBPopoverSide? Side { get; init; }
}
