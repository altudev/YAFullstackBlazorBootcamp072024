using BlazorDownloadFile;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5220/api/") });

builder.Services.AddBlazorDownloadFile(ServiceLifetime.Scoped);

await builder.Build().RunAsync();
