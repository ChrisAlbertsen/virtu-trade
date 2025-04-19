using System;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Data.DTOs.CurrentPrice;
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
        var response = await HttpClientAuthenticated.GetFromJsonAsync<Portfolio>("api/paper/create-portfolio");
        Assert.NotNull(response);
        
    }
}