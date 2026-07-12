namespace HeadlessBlazor.Docs.Client;

public static class SharedServiceConfiguration
{
    public static IServiceCollection AddSharedServices(this IServiceCollection services)
    {
        // Modal styling is applied once here as global defaults, so individual call sites
        // don't have to pass ModalOptions of their own.
        return services.AddHeadlessBlazor(configureModalDefaults: options =>
        {
            options.OverlayAttributes = new Dictionary<string, object?>
            {
                ["style"] = "position:fixed;inset:0;background:rgba(0,0,0,.5);"
            };

            options.ContentAttributes = new Dictionary<string, object?>
            {
                ["class"] = "bg-white rounded shadow p-4",
                ["style"] = "position:fixed;top:50%;left:50%;transform:translate(-50%,-50%);max-width:28rem;width:90%;z-index:1;"
            };
        });
    }
}
