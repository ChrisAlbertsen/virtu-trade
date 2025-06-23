using System;
using System.Net.Http;
using Integration.Tests.TestData.Auth;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Persistence;

namespace Integration.Tests.TestData;

public abstract class BaseIntegrationTest(
    IntegrationTestSessionFactory sessionFactory)
{
    protected readonly AppDbContext DbContext = sessionFactory.Services.CreateScope().ServiceProvider.GetRequiredService<AppDbContext>();
    protected readonly HttpClient HttpClientAuthenticated = sessionFactory.CreateClient(
        new WebApplicationFactoryClientOptions
        {
            BaseAddress = new Uri("http://localhost:5070/")
        });
    protected readonly TestDataOptions TestData = sessionFactory.Services.CreateScope().ServiceProvider.GetRequiredService<IOptions<TestDataOptions>>().Value;
}