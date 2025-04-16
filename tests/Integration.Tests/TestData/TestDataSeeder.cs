using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using Data.Entities;
using Persistence;

namespace Integration.Tests.TestData;

public class TestDataSeeder(AppDbContext context)
{
    private readonly Faker _faker = new();

    private readonly List<Portfolio> _portfolios = [];
    private readonly List<Trade> _trades = [];
    private readonly List<Holding> _holdings = [];

    public async Task SeedAsync()
    {
        await PortfolioFaker();
        await TradeFaker();
        await HoldingFaker();
        
        await context.SaveChangesAsync();
    }

    private async Task PortfolioFaker()
    {
        var portfolioFaker = new Faker<Portfolio>()
            .RuleFor(p => p.Id, f => f.Random.Guid())
            .RuleFor(p => p.Cash, f => f.Random.Decimal());
        _portfolios.AddRange(portfolioFaker.Generate(10));
        await context.Portfolios.AddRangeAsync(_portfolios);
    }

    private async Task TradeFaker()
    {
        var tradeFaker = new Faker<Trade>()
            .RuleFor(t => t.Id, f => f.Random.Guid())
            .RuleFor(t => t.PortfolioId, f => f.PickRandom(_portfolios).Id)
            .RuleFor(t => t.TradeDateTime, f => f.Date.RecentOffset(days: 30).DateTime)
            .RuleFor(t => t.Price, f => f.Finance.Amount(10m, 1000m))
            .RuleFor(t => t.Quantity, f => f.Random.Decimal(0.1m, 100m))
            .RuleFor(t => t.Symbol, f => f.Finance.Currency().Code);
        
        _trades.AddRange(tradeFaker.Generate(100));
        await context.Trades.AddRangeAsync(_trades);
    }

    private async Task HoldingFaker()
    {
        var holdingFaker = new Faker<Holding>()
            .RuleFor(h => h.Id, f => f.Random.Guid())
            .RuleFor(h => h.PortfolioId, f => f.PickRandom(_portfolios).Id)
            .RuleFor(h => h.Symbol, f => f.Finance.Currency().Code)
            .RuleFor(h => h.Quantity, f => f.Random.Decimal(1m, 500m))
            .RuleFor(h => h.AveragePurchasePrice, f => f.Finance.Amount(5m, 1000m));
        
        _holdings.AddRange(holdingFaker.Generate(100));
        await context.Holdings.AddRangeAsync(_holdings);
    }
}