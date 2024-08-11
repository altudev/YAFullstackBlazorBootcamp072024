using Blazored.Toast;
using Microsoft.Extensions.Logging;
using PasswordStorageApp.MauiClient.Services;
using Radzen;

namespace PasswordStorageApp.MauiClient
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();

            builder.Services.AddBlazoredToast();

            builder.Services.AddRadzenComponents();

            builder.Services.AddScoped<IToastService, RadzenToastManager>();

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://csharpjavadandahaiyi.tailwindcomponents.io/api/") });

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
