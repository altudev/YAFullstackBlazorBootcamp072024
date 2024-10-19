using System.Net.Http.Headers;
using Blazored.LocalStorage;
using ChatGPTClone.Application.Features.Auth.Commands.Login;

namespace ChatGPTClone.WasmClient.HttpClientDelegators;

public class HttpClientAuthDelegator : DelegatingHandler
{
    private readonly ILocalStorageService _localStorage;

    public HttpClientAuthDelegator(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var authLoginDto = await _localStorage.GetItemAsync<AuthLoginDto>("user-token");

        if (authLoginDto is not null && !string.IsNullOrEmpty(authLoginDto.Token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authLoginDto.Token);
        }

        return await base.SendAsync(request, cancellationToken);

        // var response = await base.SendAsync(request, cancellationToken);

        // if (response.StatusCode == HttpStatusCode.Unauthorized)
        // {
        // }

        // return response;
    }
}
