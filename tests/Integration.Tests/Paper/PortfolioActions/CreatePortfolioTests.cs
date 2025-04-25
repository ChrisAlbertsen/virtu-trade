using System;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Data.Entities;
using Integration.Tests.TestData;
using JetBrains.Annotations;
using Service.Paper;

namespace Integration.Tests.Paper.PortfolioActions;

[TestSubject(typeof(PaperPortfolioService))]
public class CreatePortfolioTests(IntegrationTestSessionFactory factory) : BaseIntegrationTest(factory)
{
    [Trait("Category", "Integration test")]
    [Fact(DisplayName = "Should successfully create portfolio")]
    public async Task CreatePortfolio_ShouldReturnGuid()
    {
        var response = await HttpClientAuthenticated.PostAsJsonAsync<Portfolio>("api/paper/create-portfolio", null!);
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