using System;
using System.Net.Http;
using Integration.Tests.TestData.Auth;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Persistence;

namespace Integration.Tests.TestData;

public abstract class BaseIntegrationTest : IClassFixture<IntegrationTestSessionFactory>, IDisposable
{
    private readonly IServiceScope _scope;
    protected readonly AppDbContext DbContext;
    protected readonly HttpClient HttpClientAuthenticated;
    protected readonly TestDataOptions TestData;

    protected BaseIntegrationTest(IntegrationTestSessionFactory factory)
    {
        _scope = factory.Services.CreateScope();
        DbContext = _scope.ServiceProvider.GetRequiredService<AppDbContext>();
        HttpClientAuthenticated = factory.CreateClient(
            new WebApplicationFactoryClientOptions
            {
                BaseAddress = new Uri("http://localhost:5070/")
            });
        TestData = _scope.ServiceProvider.GetRequiredService<IOptions<TestDataOptions>>().Value;
    }

    public void Dispose()
    {
        _scope.Dispose();
    }
    
}