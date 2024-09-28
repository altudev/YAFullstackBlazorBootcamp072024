using System.Text.Json;
using System.Text.Json.Serialization;
using Blazored.LocalStorage;
using ChatGPTClone.WasmClient;
using ChatGPTClone.WasmClient.AuthStateProviders;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var baseApiUrl = builder.Configuration.GetValue<string>("BaseApiUrl")!;

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(baseApiUrl) });

builder.Services.AddBlazoredLocalStorage(config =>
{
    config.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
    config.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    config.JsonSerializerOptions.IgnoreReadOnlyProperties = true;
    config.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    config.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    config.JsonSerializerOptions.ReadCommentHandling = JsonCommentHandling.Skip;
    config.JsonSerializerOptions.WriteIndented = false;
});

builder.Services.AddScoped<AuthenticationStateProvider, CustomJwtAuthStateProvider>();

builder.Services.AddAuthorizationCore();

await builder.Build().RunAsync();
