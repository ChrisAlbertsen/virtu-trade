using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Data.Entities;
using Microsoft.AspNetCore.Mvc.Testing;
using Moq;

namespace Integration.Tests.PaperController;

public class PaperControllerUnauthenticatedIntegrationTest(WebApplicationFactory<Program> factory)
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Trait("Category", "Integration test")]
    [Fact(DisplayName = "Not authenticated. Should return 401")]
    public async Task CreatePortfolio_WhenNotAuthenticated_ShouldReturnUnauthorized()
    {
        var response = await _client.PostAsync("api/paper/create-portfolio", null);
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Trait("Category", "Integration test")]
    [Fact(DisplayName = "Not authenticated. Should return 401")]
    public async Task DepositMoney_WhenNotAuthenticated_ShouldReturnUnauthorized()
    {
        var response = await _client.PostAsJsonAsync("api/paper/deposit-money", new
        {
            PortfolioId = It.IsAny<Guid>(),
            MoneyToDeposit = It.IsAny<decimal>()
        });
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Trait("Category", "Integration test")]
    [Fact(DisplayName = "Not authenticated. Should return 401")]
    public async Task GetPortfolio_WhenNotAuthenticated_ShouldReturnUnauthorized()
    {
        var response = await
            Assert.ThrowsAsync<HttpRequestException>(
                () => _client.GetFromJsonAsync<Portfolio>($"api/paper/portfolio/{It.IsAny<Guid>()}"));
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}