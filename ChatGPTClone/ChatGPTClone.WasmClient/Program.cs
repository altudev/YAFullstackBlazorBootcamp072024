using ChatGPTClone.WasmClient;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var baseApiUrl = builder.Configuration.GetValue<string>("BaseApiUrl")!;

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(baseApiUrl) });

await builder.Build().RunAsync();
