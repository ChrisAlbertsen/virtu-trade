using System.Net.Http.Json;

namespace Frontend.Api.Clients;

public class PaperApiClient(HttpClient http)
{
    public async Task CreatePortfolioAsync()
    {
        var resp = await http.PostAsync("api/Paper/create-portfolio", null);
        resp.EnsureSuccessStatusCode();
    }

    public async Task DepositMoneyAsync(Guid portfolioId, double moneyToDeposit)
    {
        var url = $"api/Paper/deposit-money?portfolioId={portfolioId}&moneyToDeposit={moneyToDeposit}";
        var resp = await http.PostAsync(url, null);
        resp.EnsureSuccessStatusCode();
    }

    public async Task<object?> GetPortfolioAsync(Guid portfolioId)
    {
        var resp = await http.GetAsync($"api/Paper/portfolio/{portfolioId}");
        resp.EnsureSuccessStatusCode();
        return await resp.Content.ReadFromJsonAsync<object>()!;
    }
}