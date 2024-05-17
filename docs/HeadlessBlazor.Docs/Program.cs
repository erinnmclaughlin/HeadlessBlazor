using HeadlessBlazor;
using HeadlessBlazor.Themes.Bootstrap;
using HeadlessBlazor.Themes.Tailwind;
using HeadlessBlazor.Docs.Components;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddHeadlessBlazor()
    .AddBootstrapTheme()
    .AddTailwindTheme();

builder.Services
    .AddRazorComponents()
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
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(HeadlessBlazor.Docs.Client._Imports).Assembly);

app.Run();
