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

namespace Integration.Tests.TestData;

public class IntegrationTestSessionFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
        .WithImage("postgres:latest")
        .WithDatabase("postgres")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseContentRoot(".");
        builder.ConfigureAppConfiguration((context, configBuilder) =>
        {
            configBuilder.AddJsonFile("TestConfigs.json", optional: false);
        });
        
        builder.ConfigureTestServices(services =>
            {
                ConfigureOptions(services);
                ConfigureAppDbService(services);
                ConfigureHttpClient(services);
            }
        );
    }

    private void ConfigureOptions(IServiceCollection services)
    {
        services.AddOptions<TestAuthOptions>()
            .BindConfiguration("TestAuthUsers");
    }

    private void ConfigureAppDbService(IServiceCollection services)
    {
        var descriptor = services
            .SingleOrDefault<ServiceDescriptor>(s => s.ServiceType == typeof(DbContextOptions<AppDbContext>));
                
        if(descriptor is not null) services.Remove(descriptor);

        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseNpgsql(_dbContainer.GetConnectionString());
        });

        services.AddScoped<TestDataSeeder, TestDataSeeder>();
    }

    private void ConfigureHttpClient(IServiceCollection services)
    {
        services.AddAuthentication("TestScheme")
            .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                "TestScheme", options => { });

        services.AddHttpClient<IBinanceApi, BinanceApi>()
            .ConfigurePrimaryHttpMessageHandler(sp =>
            {
                var config = sp.GetRequiredService<IOptions<BinanceApiSettings>>();
                return new BinanceStubHttpMessageHandler(config);
            });
    }

    public Task InitializeAsync()
    {
        return _dbContainer.StartAsync();
    }

    public new Task DisposeAsync()
    {
        return _dbContainer.StopAsync();
    }
}