using Microsoft.Extensions.DependencyInjection;
using Persistence;

namespace Integration.Tests.Utils;

public abstract class BaseIntegrationTest : IClassFixture<IntegrationTestVirtuTradeFactory>
{
    private readonly IServiceScope _scope;
    private readonly AppDbContext _dbContext;

    public BaseIntegrationTest(IntegrationTestVirtuTradeFactory factory)
    {
        _scope = factory.Services.CreateScope();
        _dbContext = _scope.ServiceProvider.GetRequiredService<AppDbContext>();
    }
}