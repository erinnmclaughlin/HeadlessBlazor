using HeadlessBlazor;
using HeadlessBlazor.Themes.Bootstrap;
using HeadlessBlazor.Themes.Tailwind;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services
    .AddHeadlessBlazor()
    .AddBootstrapTheme()
    .AddTailwindTheme();

await builder.Build().RunAsync();
