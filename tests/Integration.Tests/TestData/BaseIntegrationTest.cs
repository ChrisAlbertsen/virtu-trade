using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Persistence;

namespace Integration.Tests.TestData;

public abstract class BaseIntegrationTest(IntegrationTestSessionFactory sessionFactory)
    : IClassFixture<IntegrationTestSessionFactory>, IAsyncLifetime
{
    private readonly AppDbContext _dbContext =
        sessionFactory.Services.CreateScope().ServiceProvider.GetRequiredService<AppDbContext>();

    private readonly TestDataSeeder _seeder =
        sessionFactory.Services.CreateScope().ServiceProvider.GetRequiredService<TestDataSeeder>();

    public readonly HttpClient HttpClientAuthenticated = sessionFactory.CreateClient(
        new WebApplicationFactoryClientOptions
        {
            BaseAddress = new Uri("http://localhost:5070/")
        });

    public async Task InitializeAsync()
    {
        await _seeder.SeedAsync();
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }
}