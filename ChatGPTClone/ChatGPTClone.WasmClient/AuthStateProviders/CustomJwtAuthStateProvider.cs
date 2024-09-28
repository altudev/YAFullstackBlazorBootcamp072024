using System;
using System.Net.Http.Headers;
using System.Security.Claims;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;

namespace ChatGPTClone.WasmClient.AuthStateProviders;

public class CustomJwtAuthStateProvider : AuthenticationStateProvider
{
    private readonly ILocalStorageService _localStorage;
    private readonly HttpClient _httpClient;

    public CustomJwtAuthStateProvider(ILocalStorageService localStorage, HttpClient httpClient)
    {
        _localStorage = localStorage;
        _httpClient = httpClient;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = await _localStorage.GetItemAsync<string>("user-token");

        if (string.IsNullOrEmpty(token))
        {
            // Anonymous user
            var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());

            // Notify the authentication state changed
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(anonymousUser)));

            _httpClient.DefaultRequestHeaders.Authorization = null;

            // Return the anonymous user
            return new AuthenticationState(anonymousUser);
        }

        var claims = ParseClaimsFromJwt(token);

        var authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt"));

        var authState = new AuthenticationState(authenticatedUser);

        NotifyAuthenticationStateChanged(Task.FromResult(authState));

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        return authState;
    }

    private static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        var payload = jwt.Split('.')[1];
        var jsonBytes = ParseBase64WithoutPadding(payload);
        var keyValuePairs = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

        return keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()));
    }

    private static byte[] ParseBase64WithoutPadding(string base64)
    {
        switch (base64.Length % 4)
        {
            case 2: base64 += "=="; break;
            case 3: base64 += "="; break;
        }
        return Convert.FromBase64String(base64);
    }
}
