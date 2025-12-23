using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;
namespace MyMEDIA.GestaoLoja.Areas.Identity
{
    public sealed class RevalidatingIdentityAuthenticationStateProvider<TUser>
     : RevalidatingServerAuthenticationStateProvider where TUser : class
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IdentityOptions _options;

        public RevalidatingIdentityAuthenticationStateProvider(
            ILoggerFactory loggerFactory,
            IServiceScopeFactory scopeFactory,
            IOptions<IdentityOptions> optionsAccessor)
            : base(loggerFactory)
        {
            _scopeFactory = scopeFactory;
            _options = optionsAccessor.Value;
        }

        protected override TimeSpan RevalidationInterval => TimeSpan.FromMinutes(30);

        protected override async Task<bool> ValidateAuthenticationStateAsync(
            AuthenticationState authenticationState, CancellationToken cancellationToken)
        {
            var scope = _scopeFactory.CreateScope();
            try
            {
                var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<TUser>>();
                return await ValidateSecurityStampAsync(userMgr, authenticationState.User);
            }
            finally
            {
                if (scope is IAsyncDisposable ad) await ad.DisposeAsync();
                else scope.Dispose();
            }
        }

        private async Task<bool> ValidateSecurityStampAsync(UserManager<TUser> userManager, ClaimsPrincipal principal)
        {
            var user = await userManager.GetUserAsync(principal);
            if (user == null) return false;
            return userManager.SupportsUserSecurityStamp &&
                   principal.HasClaim(c => c.Type == ClaimTypes.Name) &&
                   principal.Claims.First(c => c.Type == ClaimTypes.Name).Value ==
                   await userManager.GetUserNameAsync(user);
        }
    }
}
