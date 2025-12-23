using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyMEDIA.Identity;
using MyMEDIA.Infraestrutura.Data;
using MyMEDIA.Shared.Services;
using MyMEDIA.GestaoLoja.Areas.Identity;
namespace MyMEDIA.GestaoLoja
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // DB
            builder.Services.AddDbContext<MyMediaDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Identity
            builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<MyMediaDbContext>();

            // Blazor Server
            builder.Services.AddServerSideBlazor();
            builder.Services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<ApplicationUser>>();

            // HTTP para API interna (opcional)
            builder.Services.AddHttpClient();

            // Autorização
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOrFunc", p => p.RequireRole("Admin", "Funcionario"));
            });

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapBlazorHub();
            app.MapFallbackToPage("/_Host");
            app.Run();
        }
    }
}
