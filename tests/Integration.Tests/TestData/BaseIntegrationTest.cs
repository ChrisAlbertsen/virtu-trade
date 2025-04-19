using System.Net.Http;
using System.Threading.Tasks;
using Infrastructure.Binance;
using Integration.Tests.BrokerController.Stubs;
using Integration.Tests.TestData.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Persistence;

namespace Integration.Tests.TestData;

public abstract class BaseIntegrationTest(IntegrationTestSessionFactory sessionFactory)
    : IClassFixture<IntegrationTestSessionFactory>, IAsyncLifetime
{
    private readonly AppDbContext _dbContext = sessionFactory.Services.CreateScope().ServiceProvider.GetRequiredService<AppDbContext>();
    private readonly TestDataSeeder _seeder = sessionFactory.Services.CreateScope().ServiceProvider.GetRequiredService<TestDataSeeder>();
    public readonly HttpClient HttpClientAuthenticated = sessionFactory.Services.CreateScope().ServiceProvider.GetRequiredService<HttpClient>();

    public async Task InitializeAsync()
    {
        await _seeder.SeedAsync();
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }
}