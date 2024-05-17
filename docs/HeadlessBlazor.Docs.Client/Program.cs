using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using HeadlessBlazor;
using HeadlessBlazor.Themes.Bootstrap;
using HeadlessBlazor.Themes.Tailwind;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services
    .AddHeadlessBlazor()
    .AddBootstrapTheme()
    .AddTailwindTheme();

await builder.Build().RunAsync();
