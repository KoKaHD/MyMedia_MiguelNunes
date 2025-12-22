using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;

namespace MyMEDIA.Frontend.Mobile.Services;

public class JwtAuthStateProvider : AuthenticationStateProvider
{
    private readonly IJSRuntime _js;
    private const string TOKENKEY = "jwtToken";
    private string? _token;

    public JwtAuthStateProvider(IJSRuntime js) => _js = js;

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        _token = await _js.GetFromLocalStorage(TOKENKEY);
        var identity = string.IsNullOrEmpty(_token)
            ? new ClaimsIdentity()
            : new ClaimsIdentity(ParseClaimsFromJwt(_token), "jwt");
        return new AuthenticationState(new ClaimsPrincipal(identity));
    }

    public async Task MarkUserAsAuthenticated(string token)
    {
        await _js.SetInLocalStorage(TOKENKEY, token);
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    public async Task MarkUserAsLoggedOut()
    {
        await _js.RemoveFromLocalStorage(TOKENKEY);
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        var handler = new JwtSecurityTokenHandler();
        var token = handler.ReadJwtToken(jwt);
        return token.Claims;
    }
}

public static class JSExt
{
    public static ValueTask<string?> GetFromLocalStorage(this IJSRuntime js, string key)
        => js.InvokeAsync<string?>("localStorage.getItem", key);

    public static ValueTask SetInLocalStorage(this IJSRuntime js, string key, string value)
        => js.InvokeVoidAsync("localStorage.setItem", key, value);

    public static ValueTask RemoveFromLocalStorage(this IJSRuntime js, string key)
        => js.InvokeVoidAsync("localStorage.removeItem", key);
}