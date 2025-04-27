using System;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Persistence;

namespace Integration.Tests.TestData;

public abstract class BaseIntegrationTest(IntegrationTestSessionFactory sessionFactory)
{
    protected readonly AppDbContext DbContext =
        sessionFactory.Services.CreateScope().ServiceProvider.GetRequiredService<AppDbContext>();

    public readonly HttpClient HttpClientAuthenticated = sessionFactory.CreateClient(
        new WebApplicationFactoryClientOptions
        {
            BaseAddress = new Uri("http://localhost:5070/")
        });
}