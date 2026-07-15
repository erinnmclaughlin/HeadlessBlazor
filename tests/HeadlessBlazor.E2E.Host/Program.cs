using HeadlessBlazor;
using HeadlessBlazor.E2E.Host.Components;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddHeadlessBlazor();

var app = builder.Build();

// No HTTPS redirect on purpose: the E2E fixture drives this over plain HTTP so CI never needs a
// dev certificate.
app.UseAntiforgery();
app.MapStaticAssets();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(HeadlessBlazor.E2E.Host.Client._Imports).Assembly);

// Lets the test fixture poll for readiness instead of guessing at a startup delay.
app.MapGet("/healthz", () => Results.Ok("ok"));

app.Run();
