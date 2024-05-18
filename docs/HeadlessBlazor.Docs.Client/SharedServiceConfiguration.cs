namespace HeadlessBlazor.Docs.Client;

public static class SharedServiceConfiguration
{
    public static IServiceCollection AddSharedServices(this IServiceCollection services)
    {
        return services.AddHeadlessBlazor();
    }
}
