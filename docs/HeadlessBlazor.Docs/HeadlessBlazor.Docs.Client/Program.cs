using HeadlessBlazor;
using HeadlessBlazor.Core.Themes;
using HeadlessBlazor.Docs.Client;
using HeadlessBlazor.Themes.Bootstrap;
using HeadlessBlazor.Themes.Tailwind;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Options;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddScoped<HeadManipulationService>();

builder.Services.AddHeadlessBlazor()
    .AddBootstrapTheme()
    .AddTailwindTheme()
    .AddSingleton<HBThemeFactory>()
    .AddSingleton(sp =>
    {
        var theme = sp.GetRequiredService<IOptions<HBThemeFactory>>().Value.Theme;
        return sp.GetRequiredKeyedService<HBTheme>(theme);
    });

await builder.Build().RunAsync();
