using HeadlessBlazor;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddHeadlessBlazor();

await builder.Build().RunAsync();
