using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Persistence;
using Testcontainers.PostgreSql;

namespace Integration.Tests.TestData;

public class IntegrationTestAppDbFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
        .WithImage("postgres:latest")
        .WithDatabase("postgres")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
            {
                var descriptor = Enumerable
                    .SingleOrDefault<ServiceDescriptor>(services, s => s.ServiceType == typeof(DbContextOptions<AppDbContext>));
                
                if(descriptor is not null) services.Remove(descriptor);

                EntityFrameworkServiceCollectionExtensions.AddDbContext<AppDbContext>(services, options =>
                {
                    NpgsqlDbContextOptionsBuilderExtensions.UseNpgsql(options, _dbContainer.GetConnectionString());
                });
            }
        );
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