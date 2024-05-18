namespace HeadlessBlazor.Behaviors;

public interface ICloseable : IReferenceable
{
    Task CloseAsync();
}
