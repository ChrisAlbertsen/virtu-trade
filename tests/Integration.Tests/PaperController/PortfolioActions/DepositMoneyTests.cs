using System;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Api.Controllers;
using Data.Entities;
using Integration.Tests.TestData;
using JetBrains.Annotations;

namespace Integration.Tests.Paper.PortfolioActions;

[Collection("IntegrationTest")]
[TestSubject(typeof(PaperController))]
public class DepositMoneyTests(IntegrationTestSessionFactory factory) : BaseIntegrationTest(factory)
{
    private const decimal DepositAmount = 100m;

    [Trait("Category", "Integration test")]
    [Fact(DisplayName = "Should successfully deposit paper money to portfolio")]
    public async Task DepositMoney_ShouldSuccessfullyDepositMoney()
    {
        var portfolio = await CreatePortfolio();

        var depositResponse =
            await HttpClient.PostAsync(
                $"api/paper/deposit-money?portfolioId={portfolio.Id}&moneyToDeposit={DepositAmount}", null);

        Assert.NotNull(depositResponse);
        Assert.True(depositResponse.IsSuccessStatusCode);

        var persistedPortfolio = await DbContext
            .Portfolios
            .FindAsync(portfolio.Id);
        Assert.NotNull(persistedPortfolio);

        Assert.Equal(DepositAmount, persistedPortfolio.Cash);
    }

    [Trait("Category", "Integration test")]
    [Fact(DisplayName = "Should fail to deposit paper money to portfolio")]
    public async Task? DepositMoney_WhenPortfolioDoesNotExist_ShouldReturnError()
    {
        var depositResponse =
            await HttpClient.PostAsync(
                $"api/paper/deposit-money?portfolioId={Guid.NewGuid()}&moneyToDeposit={DepositAmount}", null);

        Assert.NotNull(depositResponse);
        Assert.False(depositResponse.IsSuccessStatusCode);
        Assert.True(depositResponse.StatusCode == HttpStatusCode.Forbidden);
    }

    private async Task<Portfolio> CreatePortfolio()
    {
        var portfolioResponse = await HttpClient.PostAsJsonAsync<Portfolio>("api/paper/create-portfolio", null!);
        Assert.NotNull(portfolioResponse);
        Assert.True(portfolioResponse.IsSuccessStatusCode);

        var portfolio = await portfolioResponse
            .Content
            .ReadFromJsonAsync<Portfolio>();
        Assert.NotNull(portfolio);
        return portfolio;
    }
}