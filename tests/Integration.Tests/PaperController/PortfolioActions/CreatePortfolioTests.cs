using System;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Api.Controllers;
using Data.Entities;
using Integration.Tests.TestData;
using JetBrains.Annotations;

namespace Integration.Tests.Paper.PortfolioActions;

[Collection("IntegrationTest")]
[TestSubject(typeof(PaperController))]
public class CreatePortfolioTests(IntegrationTestSessionFactory factory) : BaseIntegrationTest(factory)
{
    [Trait("Category", "Integration test")]
    [Fact(DisplayName = "Should successfully create portfolio")]
    public async Task CreatePortfolio_ShouldReturnCreatedPortfolio()
    {
        var response = await HttpClient.PostAsJsonAsync<Portfolio>("api/paper/create-portfolio", null!);
        Assert.NotNull(response);
        Assert.True(response.IsSuccessStatusCode);

        var portfolio = await response
            .Content
            .ReadFromJsonAsync<Portfolio>();
        Assert.NotNull(portfolio);

        var persistedPortfolio = await DbContext
            .Portfolios
            .FindAsync(portfolio.Id);
        Assert.NotNull(persistedPortfolio);

        Assert.Equal(portfolio.Cash, persistedPortfolio.Cash);
        Assert.Equal(portfolio.ReservedCash, persistedPortfolio.ReservedCash);
    }

    [Trait("Category", "Integration test")]
    [Fact(DisplayName = "Should create an empty portfolio")]
    public async Task CreatePortfolio_ShouldCreateEmptyPortfolio()
    {
        var response = await HttpClient.PostAsJsonAsync<Portfolio>("api/paper/create-portfolio", null!);
        Assert.NotNull(response);
        Assert.True(response.IsSuccessStatusCode);

        var portfolio = await response
            .Content
            .ReadFromJsonAsync<Portfolio>();

        Assert.NotNull(portfolio);
        Assert.NotEqual(Guid.Empty, portfolio.Id);
        Assert.Equal(0, portfolio.Cash);
        Assert.Equal(0, portfolio.ReservedCash);
        Assert.Empty(portfolio.Holdings);
        Assert.Empty(portfolio.Trades);
    }
}