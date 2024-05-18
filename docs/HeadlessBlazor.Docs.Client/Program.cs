using Blazored.LocalStorage;
using HeadlessBlazor.Docs.Client;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddSharedServices();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<IRazorFileReader, HttpRazorFileReader>();

builder.Services.AddBlazoredLocalStorage();

await builder.Build().RunAsync();
