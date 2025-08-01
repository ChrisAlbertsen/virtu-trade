﻿using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Data.DTOs.Orders;
using Integration.Tests.TestData;
using Integration.Tests.TestData.Factories;
using Moq;

namespace Integration.Tests.BrokerController.Auth;

[Collection("UnauthenticatedIntegrationTest")]
public class BrokerControllerUnauthenticatedIntegrationTest(UnauthenticatedIntegrationTestSessionFactory factory)
    : BaseIntegrationTest(factory)
{
    [Trait("Category", "Integration test")]
    [Theory(DisplayName = "Not authenticated. Should return 401")]
    [InlineData("prices/current?symbol=BTCUSDT")]
    [InlineData("prices/historical?symbol=BTCUSDT&interval=1h")]
    public async Task GetEndpoints_WhenNotAuthenticated_ShouldReturnUnauthorized(string url)
    {
        var response = await Assert
            .ThrowsAsync<HttpRequestException>(
                () => HttpClient.GetFromJsonAsync<object>($"api/broker/{url}"));
        Assert.Equal("Response status code does not indicate success: 401 (Unauthorized).", response.Message);
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Trait("Category", "Integration test")]
    [Theory(DisplayName = "Not authenticated. Should return 401")]
    [InlineData("orders/execute-market-order")]
    public async Task PostEndpoints_WhenNotAuthenticated_ShouldReturnUnauthorized(string url)
    {
        var marketOrder = new MarketOrderParams
        {
            PortfolioId = It.IsAny<Guid>(),
            Quantity = 10,
            Symbol = "TICKER"
        };
        var response =
            await HttpClient.PostAsJsonAsync($"api/broker/{url}", marketOrder);
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}