using Microsoft.AspNetCore.Components.Authorization;
using MyMEDIA.Frontend.Mobile.Services;
using MyMEDIA.Shared.Services;
namespace MyMEDIA.Frontend.Mobile
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

            // Serviços compartilhados
            builder.Services.AddMauiBlazorWebView();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
#endif

            // HTTP
            builder.Services.AddScoped(sp => new HttpClient
            {
                BaseAddress = new Uri("https://10.0.2.2:5001") // Android emulator
                                                               // BaseAddress = new Uri("https://localhost:5001") // Windows/iOS
            });

            // Auth + Carrinho
            builder.Services.AddScoped<ApiService>();
            builder.Services.AddScoped<AuthService>();
            builder.Services.AddScoped<AuthenticationStateProvider, JwtAuthStateProvider>();
            builder.Services.AddAuthorizationCore();
            builder.Services.AddSingleton<CarrinhoState>();

            return builder.Build();
        }
    }
}
