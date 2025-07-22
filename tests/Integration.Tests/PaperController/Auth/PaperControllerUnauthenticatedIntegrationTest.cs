using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Data.Entities;
using Integration.Tests.TestData;
using Integration.Tests.TestData.Factories;
using Moq;

namespace Integration.Tests.Paper.Auth;

[Collection("UnauthenticatedIntegrationTest")]
public class PaperControllerUnauthenticatedIntegrationTest(UnauthenticatedIntegrationTestSessionFactory factory)
    : BaseIntegrationTest(factory)
{
    [Trait("Category", "Integration test")]
    [Fact(DisplayName = "Not authenticated. Should return 401")]
    public async Task CreatePortfolio_WhenNotAuthenticated_ShouldReturnUnauthorized()
    {
        var response = await HttpClient.PostAsync("api/paper/create-portfolio", null);
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Trait("Category", "Integration test")]
    [Fact(DisplayName = "Not authenticated. Should return 401")]
    public async Task DepositMoney_WhenNotAuthenticated_ShouldReturnUnauthorized()
    {
        var response = await HttpClient.PostAsJsonAsync("api/paper/deposit-money", new
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
                () => HttpClient.GetFromJsonAsync<Portfolio>($"api/paper/portfolio/{It.IsAny<Guid>()}"));
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}