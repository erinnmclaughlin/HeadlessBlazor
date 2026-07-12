namespace HeadlessBlazor.Docs.Client;

public static class SharedServiceConfiguration
{
    public static IServiceCollection AddSharedServices(this IServiceCollection services)
    {
        // Modal styling is applied once here as global defaults, so individual call sites
        // don't have to pass ModalOptions of their own. The classes below live in app.css and
        // transition off the data-state attribute the host sets while TransitionDuration is set.
        return services.AddHeadlessBlazor(configureModalDefaults: options =>
        {
            options.TransitionDuration = TimeSpan.FromMilliseconds(200);

            options.OverlayAttributes = new Dictionary<string, object?>
            {
                ["class"] = "hb-modal-overlay"
            };

            options.ContentAttributes = new Dictionary<string, object?>
            {
                ["class"] = "hb-modal-dialog bg-white rounded shadow p-4"
            };
        });
    }
}
