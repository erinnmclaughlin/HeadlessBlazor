using HeadlessBlazor;
using HeadlessBlazor.Docs.Client;
using HeadlessBlazor.Docs.Components;
using HeadlessBlazor.Themes.Bootstrap;
using HeadlessBlazor.Themes.Tailwind;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<HeadManipulationService>();

builder.Services
    .AddHeadlessBlazor()
    .AddBootstrapTheme()
    .AddTailwindTheme();

builder.Services
    .AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(HeadlessBlazor.Docs.Client._Imports).Assembly);

app.Run();
