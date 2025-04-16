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

public abstract class BaseIntegrationTest : IClassFixture<IntegrationTestAppDbFactory>, IAsyncLifetime
{
    private readonly IServiceScope _scope;
    private readonly AppDbContext _dbContext;
    private readonly TestDataSeeder _seeder;
    public readonly HttpClient HttpClientAuthenticated;

    public BaseIntegrationTest(IntegrationTestAppDbFactory factory)
    {
        _scope = factory.Services.CreateScope();
        _dbContext = _scope.ServiceProvider.GetRequiredService<AppDbContext>();
        _seeder = new TestDataSeeder(_dbContext);
        HttpClientAuthenticated = CreateAuthenticatedHttpClient(factory);
    }

    private HttpClient CreateAuthenticatedHttpClient(WebApplicationFactory<Program> factory)
    {
        var client = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureAppConfiguration((hostingContext, config) =>
            {
                config.AddJsonFile("TestConfigs.json");
            });
            
            builder.ConfigureTestServices(services =>
            {
                services.AddOptions<TestAuthOptions>()
                    .BindConfiguration("TestAuthUser");
                
                services.AddAuthentication("TestScheme")
                    .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                        "TestScheme", options => { });

                services.AddHttpClient<IBinanceApi, BinanceApi>()
                    .ConfigurePrimaryHttpMessageHandler(sp =>
                    {
                        var config = sp.GetRequiredService<IOptions<BinanceApiSettings>>();
                        return new BinanceStubHttpMessageHandler(config);
                    });
            });
        }).CreateClient();

        return client;
    }

    public async Task InitializeAsync()
    {
        await _seeder.SeedAsync();
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }
}