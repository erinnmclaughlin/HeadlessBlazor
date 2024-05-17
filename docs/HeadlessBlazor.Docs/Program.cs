using HeadlessBlazor.Docs;
using HeadlessBlazor.Docs.Client;
using HeadlessBlazor.Docs.Components;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddClientServices()
    .AddRazorComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddScoped<IFileProvider, DirectoryFileProvider>();

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

app.MapGet("api/files/{fileName}", async (string fileName, IFileProvider fileProvider) =>
{
    var content = await fileProvider.GetFilesAsync($"{fileName}.razor");
    return string.IsNullOrEmpty( content ) ? Results.NotFound() : Results.Ok(content);
});

app.Run();
