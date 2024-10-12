using System;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using Blazored.LocalStorage;
using ChatGPTClone.Application.Common.Models.Errors;
using ChatGPTClone.Application.Common.Models.General;
using ChatGPTClone.Application.Features.Auth.Commands.Login;
using ChatGPTClone.Application.Features.Auth.Commands.RefreshToken;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace ChatGPTClone.WasmClient.AuthStateProviders;

public class CustomJwtAuthStateProvider : AuthenticationStateProvider
{
    private readonly ILocalStorageService _localStorage;
    private readonly HttpClient _httpClient;
    private readonly NavigationManager _navigationManager;

    public CustomJwtAuthStateProvider(ILocalStorageService localStorage, HttpClient httpClient, NavigationManager navigationManager)
    {
        _localStorage = localStorage;
        _httpClient = httpClient;
        _navigationManager = navigationManager;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var authLoginDto = await _localStorage.GetItemAsync<AuthLoginDto>("user-token");

        if (authLoginDto is null || string.IsNullOrEmpty(authLoginDto.Token) || authLoginDto.RefreshTokenExpiresAt < DateTime.UtcNow)
        {
            // Anonymous user
            var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());

            // Notify the authentication state changed
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(anonymousUser)));

            _httpClient.DefaultRequestHeaders.Authorization = null;

            await _localStorage.RemoveItemAsync("user-token");

            // Return the anonymous user
            return new AuthenticationState(anonymousUser);
        }

        var cts = new CancellationTokenSource();

        var refreshTokenResponse = await RefreshTokenAsync(authLoginDto, cts.Token);

        if (!refreshTokenResponse.Success)
        {
            await _localStorage.RemoveItemAsync("user-token");

            var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(anonymousUser)));

            _httpClient.DefaultRequestHeaders.Authorization = null;

            _navigationManager.NavigateTo("/auth/login");

            return new AuthenticationState(anonymousUser);
        }

        var claims = ParseClaimsFromJwt(refreshTokenResponse.Data.Token);

        var authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt"));

        var authState = new AuthenticationState(authenticatedUser);

        NotifyAuthenticationStateChanged(Task.FromResult(authState));

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", refreshTokenResponse.Data.Token);

        await _localStorage.SetItemAsync("user-token", new AuthLoginDto(refreshTokenResponse.Data.Token, refreshTokenResponse.Data.ExpiresAt, authLoginDto.RefreshToken, authLoginDto.RefreshTokenExpiresAt));

        return authState;
    }

    private async Task<ResponseDto<AuthRefreshTokenResponse>> RefreshTokenAsync(AuthLoginDto authLoginDto, CancellationToken cancellationToken)
    {
        var refreshTokenCommand = new AuthRefreshTokenCommand(authLoginDto.Token, authLoginDto.RefreshToken);

        try
        {
            var response = await _httpClient
            .PostAsJsonAsync("auth/refresh-token", refreshTokenCommand, cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                var responseDto = await response
                    .Content
                    .ReadFromJsonAsync<ResponseDto<AuthRefreshTokenResponse>>(cancellationToken: cancellationToken);

                return responseDto!;
            }

            return new ResponseDto<AuthRefreshTokenResponse>("Refresh token failed", new List<ErrorDto>());
        }
        catch (Exception)
        {
            return new ResponseDto<AuthRefreshTokenResponse>("Refresh token failed", new List<ErrorDto>());
        }
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
