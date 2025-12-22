using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MyMEDIA.Shared.Services;

namespace MyMEDIA.Frontend
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            // HTTP + JWT
            builder.Services.AddScoped(sp => new HttpClient
            {
                BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
            });

            builder.Services.AddScoped<ApiService>();
            builder.Services.AddScoped<AuthService>();

            // Carrinho (singleton)
            builder.Services.AddSingleton<CarrinhoState>();

            // Auth com JWT
            builder.Services.AddScoped<AuthenticationStateProvider, JwtAuthStateProvider>();
            builder.Services.AddAuthorizationCore();

            await builder.Build().RunAsync();
        }
    }
}
