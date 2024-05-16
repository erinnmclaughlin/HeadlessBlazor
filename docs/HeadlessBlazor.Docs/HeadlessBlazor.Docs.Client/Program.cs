using HeadlessBlazor;
using HeadlessBlazor.Themes.Bootstrap;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.Services.AddHeadlessBlazor(o => o.UseBootstrap());
await builder.Build().RunAsync();
