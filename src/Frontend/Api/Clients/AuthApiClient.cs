using System.Net.Http.Json;
using Frontend.Api.Models;

namespace Frontend.Api.Clients;

public class AuthApiClient(HttpClient http)
{
    public async Task RegisterAsync(RegisterRequest req)
        {
            var resp = await http.PostAsJsonAsync("register", req);
            resp.EnsureSuccessStatusCode();
        }

        public async Task<AccessTokenResponse?> LoginAsync(LoginRequest req, bool useCookies = false, bool useSessionCookies = false)
        {
            var url = $"login?useCookies={useCookies}&useSessionCookies={useSessionCookies}";
            var resp = await http.PostAsJsonAsync(url, req);
            resp.EnsureSuccessStatusCode();
            return await resp.Content.ReadFromJsonAsync<AccessTokenResponse>()!;
        }

        public async Task<AccessTokenResponse?> RefreshAsync(RefreshRequest req)
        {
            var resp = await http.PostAsJsonAsync("refresh", req);
            resp.EnsureSuccessStatusCode();
            return await resp.Content.ReadFromJsonAsync<AccessTokenResponse>()!;
        }

        public async Task ConfirmEmailAsync(string userId, string code, string? changedEmail = null)
        {
            var url = $"confirmEmail?userId={Uri.EscapeDataString(userId)}&code={Uri.EscapeDataString(code)}"
                      + (changedEmail is not null ? $"&changedEmail={Uri.EscapeDataString(changedEmail)}" : "");
            var resp = await http.GetAsync(url);
            resp.EnsureSuccessStatusCode();
        }

        public async Task ResendConfirmationEmailAsync(ResendConfirmationEmailRequest req)
        {
            var resp = await http.PostAsJsonAsync("resendConfirmationEmail", req);
            resp.EnsureSuccessStatusCode();
        }

        public async Task ForgotPasswordAsync(ForgotPasswordRequest req)
        {
            var resp = await http.PostAsJsonAsync("forgotPassword", req);
            resp.EnsureSuccessStatusCode();
        }

        public async Task ResetPasswordAsync(ResetPasswordRequest req)
        {
            var resp = await http.PostAsJsonAsync("resetPassword", req);
            resp.EnsureSuccessStatusCode();
        }

        public async Task<TwoFactorResponse?> Manage2FAAsync(TwoFactorRequest req)
        {
            var resp = await http.PostAsJsonAsync("manage/2fa", req);
            resp.EnsureSuccessStatusCode();
            return await resp.Content.ReadFromJsonAsync<TwoFactorResponse>()!;
        }

        public async Task<InfoResponse?> GetInfoAsync()
        {
            var resp = await http.GetAsync("manage/info");
            resp.EnsureSuccessStatusCode();
            return await resp.Content.ReadFromJsonAsync<InfoResponse>()!;
        }

        public async Task<InfoResponse?> UpdateInfoAsync(InfoRequest req)
        {
            var resp = await http.PostAsJsonAsync("manage/info", req);
            resp.EnsureSuccessStatusCode();
            return await resp.Content.ReadFromJsonAsync<InfoResponse>()!;
        }
    }