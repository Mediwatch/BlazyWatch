using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using Mediwatch.Shared;

namespace Mediwatch.Client.services
{
    public class MediwatchAuthentifiacationProvider : AuthenticationStateProvider
    {
        private readonly HttpClient _httpClient;
        private UserInformation _userInfoCache;

        private async Task<UserInformation> GetUserInfo()
        {
            if (_userInfoCache != null && _userInfoCache.IsAuthenticated)
                return _userInfoCache;
            _userInfoCache = await _httpClient.GetFromJsonAsync<UserInformation>("Account/UserInfo");
            return _userInfoCache;
        }

        public async Task AddToTutor(string userName)
        {
            var path = "Account/AddTutor?username=" + userName;
            var result = await _httpClient.GetAsync(path);
            if (result.StatusCode == System.Net.HttpStatusCode.BadRequest) throw new Exception(await result.Content.ReadAsStringAsync());
        }

        public async Task Login(LoginForm loginForm)
        {
            var stringContent = new StringContent(JsonSerializer.Serialize(loginForm), Encoding.UTF8, "application/json");
            var result = await _httpClient.PostAsync("Account/Login", stringContent);
            if (result.StatusCode == System.Net.HttpStatusCode.BadRequest) throw new Exception(await result.Content.ReadAsStringAsync());
            result.EnsureSuccessStatusCode();
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public async Task LoginExternal()
        {
            var stringContent = new StringContent(JsonSerializer.Serialize("hey"), Encoding.UTF8, "application/json");
            var result = await _httpClient.PostAsync("Account/LoginExternal", stringContent);
            if (result.StatusCode == System.Net.HttpStatusCode.BadRequest) throw new Exception(await result.Content.ReadAsStringAsync());
            result.EnsureSuccessStatusCode();
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public async Task Logout()
        {
            var result = await _httpClient.PostAsync("Account/Logout", null);
            result.EnsureSuccessStatusCode();
            _userInfoCache = null;
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public async Task Register(RegisterForm registerForm)
        {
            var stringContent = new StringContent(JsonSerializer.Serialize(registerForm), Encoding.UTF8, "application/json");
            var result = await _httpClient.PostAsync("Account/Register", stringContent);
            if (result.StatusCode == System.Net.HttpStatusCode.BadRequest) throw new Exception(await result.Content.ReadAsStringAsync());
            result.EnsureSuccessStatusCode();
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public MediwatchAuthentifiacationProvider(HttpClient httpClient)
        {
            this._httpClient = httpClient;
        }


        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var identity = new ClaimsIdentity();
            try
            {
                var userInfo = await GetUserInfo();
                if (userInfo.IsAuthenticated)
                {
                    var claims = new[] { new Claim(ClaimTypes.Name, _userInfoCache.UserName) }.Concat(_userInfoCache.Claims.Select(c => new Claim(c.Key, c.Value)));
                    identity = new ClaimsIdentity(claims, "Server authentication");
                }
            }
            catch (HttpRequestException exeption)
            {
                Console.WriteLine("Request failed:" + exeption.ToString());
            }
            var claimIdentity = new AuthenticationState(new ClaimsPrincipal(identity));
             Console.WriteLine(claimIdentity.User.Identity.IsAuthenticated);
            return claimIdentity;
        }
    }
}