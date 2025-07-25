﻿using System;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Api.Controllers;
using Data.Entities;
using Integration.Tests.TestData;
using Integration.Tests.TestData.Factories;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace Integration.Tests.Paper.PortfolioActions;

[TestSubject(typeof(PaperController))]
[Collection("IntegrationTest")]
public class GetPortfolioTests(IntegrationTestSessionFactory factory) : BaseIntegrationTest(factory)
{
    [Trait("Category", "Integration test")]
    [Fact(DisplayName = "Should successfully return portfolio")]
    public async Task GetPortfolio_ShouldReturnPortfolio()
    {
        var response =
            await HttpClient.GetAsync($"api/paper/portfolio/{TestData.TestAuthUserAuthenticated.PortfolioId}");

        Assert.NotNull(response);
        Assert.True(response.IsSuccessStatusCode);
        var portfolio = await response
            .Content
            .ReadFromJsonAsync<Portfolio>();

        Assert.NotNull(portfolio);

        var persistedPortfolio = await DbContext
            .Portfolios
            .Include(p => p.Holdings)
            .FirstOrDefaultAsync(p => p.Id == new Guid(TestData.TestAuthUserAuthenticated.PortfolioId));

        Assert.NotNull(persistedPortfolio);

        Assert.Equivalent(portfolio, persistedPortfolio);
    }

    [Trait("Category", "Integration test")]
    [Fact(DisplayName = "Should fail to get not existing portfolio")]
    public async Task? GetPortfolio_WhenPortfolioDoesNotExist_ShouldReturnForbidden()
    {
        var response = await HttpClient.GetAsync($"api/paper/portfolio/{Guid.Empty}");

        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Trait("Category", "Integration test")]
    [Fact(DisplayName = "Should fail to get other user's portfolio")]
    public async Task? GetPortfolio_WhenRequestedByUnauthenticatedUser_ShouldReturnForbidden()
    {
        var response =
            await HttpClient.GetAsync($"api/paper/portfolio/{TestData.TestAuthUserUnauthenticated.PortfolioId}");

        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }
}