using System.Net.Http.Headers;
using System.Security.Claims;
using Blazored.LocalStorage;
using ClienteBlogWASM.Helpers;
using Microsoft.AspNetCore.Components.Authorization;

namespace ClienteBlogWASM.Servicios
{
    public class AuthStateProvider : AuthenticationStateProvider
    {
        private readonly HttpClient _httpClient;

        private readonly ILocalStorageService _localStorageService;

        public AuthStateProvider(HttpClient client, ILocalStorageService localStorageService)
        {
            _httpClient = client;
            _localStorageService = localStorageService;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await _localStorageService.GetItemAsync<string>(Initialize.Token_Local);
                if (token == null)
                {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
                }
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(JwtParser.ParseClaimsFromJwt(token), "jwtAuthType")));
        }

        public void NotificarUsuarioLogueado(string token)
        {
            var authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity(JwtParser.ParseClaimsFromJwt(token), "jwtAuthType"));
            var authState = Task.FromResult(new AuthenticationState(authenticatedUser)); 
            NotifyAuthenticationStateChanged(authState);
        }

        public void NotificarUsuarioSalir()
        {
            var authState = Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())));
            NotifyAuthenticationStateChanged(authState);
        }
    }
}