using Microsoft.AspNetCore.Components.Authorization;
using MyMEDIA.API.DTOs;
using System.Net.Http.Json;

namespace MyMEDIA.Frontend.Mobile.Services;

public class AuthService
{
    private readonly HttpClient _http;
    private readonly AuthenticationStateProvider _auth;

    public AuthService(HttpClient http, AuthenticationStateProvider auth)
    {
        _http = http;
        _auth = auth;
    }

    public async Task<bool> LoginAsync(string email, string password)
    {
        var res = await _http.PostAsJsonAsync("api/auth/login", new LoginRequest { Email = email, Password = password });
        if (!res.IsSuccessStatusCode) return false;

        var token = await res.Content.ReadFromJsonAsync<LoginResponse>();
        await ((JwtAuthStateProvider)_auth).MarkUserAsAuthenticated(token!.Token);
        return true;
    }

    public async Task LogoutAsync()
    {
        await ((JwtAuthStateProvider)_auth).MarkUserAsLoggedOut();
    }
}