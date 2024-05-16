using HeadlessBlazor;
using HeadlessBlazor.Docs.Components;
using HeadlessBlazor.Themes.Bootstrap;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddHeadlessBlazor(o => o.UseBootstrap())
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
