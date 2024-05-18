using HeadlessBlazor.Utilities;
using Microsoft.Extensions.DependencyInjection;

namespace HeadlessBlazor;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddHeadlessBlazor(this IServiceCollection services)
    {
        services.AddTransient<DOM>();
        return services;
    }
}
