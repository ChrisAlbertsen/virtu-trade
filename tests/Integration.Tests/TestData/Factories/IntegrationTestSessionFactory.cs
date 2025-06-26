using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Binance;
using Integration.Tests.BrokerController.Stubs;
using Integration.Tests.TestData.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Persistence;
using Testcontainers.PostgreSql;

namespace Integration.Tests.TestData.Factories;

public class IntegrationTestSessionFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
        .WithImage("postgres:latest")
        .WithDatabase("postgres")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .Build();

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();

        var testDataSeeder = Services.CreateScope().ServiceProvider.GetRequiredService<TestDataSeeder>();
        await testDataSeeder.SeedAsync();
    }

    public new Task DisposeAsync()
    {
        return _dbContainer.StopAsync();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseContentRoot(".");
        builder.ConfigureAppConfiguration((context, configBuilder) =>
        {
            configBuilder.AddJsonFile("TestConfigs.json", false);
        });

        builder.ConfigureTestServices(services =>
            {
                ConfigureOptions(services);
                ConfigureAppDbService(services);
                ConfigureAuth(services);
                ConfigureBinanceApi(services);
            }
        );
    }

    private void ConfigureOptions(IServiceCollection services)
    {
        services.AddOptions<TestDataOptions>()
            .BindConfiguration("TestAuthUsers");
    }

    private void ConfigureAppDbService(IServiceCollection services)
    {
        var descriptor = services
            .SingleOrDefault(s => s.ServiceType == typeof(DbContextOptions<AppDbContext>));

        if (descriptor is not null) services.Remove(descriptor);

        services.AddDbContext<AppDbContext>(options => { options.UseNpgsql(_dbContainer.GetConnectionString()); });

        services.AddScoped<TestDataSeeder, TestDataSeeder>();
    }

    protected virtual void ConfigureAuth(IServiceCollection services)
    {
        services.AddAuthentication("TestScheme")
            .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                "TestScheme", options => { });
    }

    private void ConfigureBinanceApi(IServiceCollection services)
    {
        services.AddHttpClient<IBinanceApi, BinanceApi>()
            .ConfigurePrimaryHttpMessageHandler(sp =>
            {
                var config = sp.GetRequiredService<IOptions<BinanceApiSettings>>();
                return new BinanceStubHttpMessageHandler(config);
            });
    }
}