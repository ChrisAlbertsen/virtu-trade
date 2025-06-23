using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bogus;
using Data.AuthModels;
using Data.Entities;
using Integration.Tests.TestData.Auth;
using Microsoft.Extensions.Options;
using Persistence;

namespace Integration.Tests.TestData;

public class TestDataSeeder(AppDbContext context, IOptions<TestDataOptions> options)
{
    private readonly List<Holding> _holdings = [];
    private readonly List<Portfolio> _portfolios = [];
    private readonly List<Trade> _trades = [];
    private readonly List<UserPortfolioAccess> _userPortfolioAccesses = [];
    private readonly List<User> _users = [];

    public async Task SeedAsync()
    {
        await PortfolioFaker();
        await TradeFaker();
        await HoldingFaker();
        await UserFaker();
        await UserPortfolioAccessFaker();

        await context.SaveChangesAsync();
    }

    private async Task PortfolioFaker()
    {
        var portfolioFaker = new Faker<Portfolio>()
            .RuleFor(p => p.Id, f => f.Random.Guid())
            .RuleFor(p => p.Cash, f => f.Random.Decimal());
        _portfolios.AddRange(portfolioFaker.Generate(10));

        _portfolios.Add(new Portfolio
        {
            Id = Guid.Parse(options.Value.TestAuthUserA.PortfolioId),
            Cash = 10000,
            ReservedCash = 0
        });

        _portfolios.Add(new Portfolio
        {
            Id = Guid.Parse(options.Value.TestAuthUserB.PortfolioId),
            Cash = 10000,
            ReservedCash = 0
        });

        await context.Portfolios.AddRangeAsync(_portfolios);
    }

    private async Task TradeFaker()
    {
        var tradeFaker = new Faker<Trade>()
            .RuleFor(t => t.Id, f => f.Random.Guid())
            .RuleFor(t => t.PortfolioId, f => f.PickRandom(_portfolios).Id)
            .RuleFor(t => t.TradeDateTime, f => f.Date.RecentOffset(30).UtcDateTime)
            .RuleFor(t => t.Price, f => f.Finance.Amount(10m))
            .RuleFor(t => t.Quantity, f => f.Random.Decimal(0.1m, 100m))
            .RuleFor(t => t.Symbol, f => f.Finance.Currency().Code);

        _trades.AddRange(tradeFaker.Generate(40));
        await context.Trades.AddRangeAsync(_trades);
    }

    private async Task HoldingFaker()
    {
        var holdingFaker = new Faker<Holding>()
            .RuleFor(h => h.Id, f => f.Random.Guid())
            .RuleFor(h => h.PortfolioId, f => f.PickRandom(_portfolios).Id)
            .RuleFor(h => h.Symbol, f => f.Finance.Currency().Code)
            .RuleFor(h => h.Quantity, f => f.Random.Decimal(1m, 500m))
            .RuleFor(h => h.AveragePurchasePrice, f => f.Finance.Amount(5m));

        _holdings.AddRange(holdingFaker.Generate(20));
        await context.Holdings.AddRangeAsync(_holdings);
    }

    private async Task UserFaker()
    {
        var userFaker = new Faker<User>()
            .RuleFor(u => u.UserName, f => f.Internet.UserName())
            .RuleFor(u => u.Email, f => f.Internet.Email());

        _users.AddRange(userFaker.Generate(5));

        _users.Add(new User
        {
            Id = options.Value.TestAuthUserA.UserId
        });

        _users.Add(new User
        {
            Id = options.Value.TestAuthUserB.UserId
        });

        await context.Users.AddRangeAsync(_users);
    }

    private async Task UserPortfolioAccessFaker()
    {
        _userPortfolioAccesses.Add(new UserPortfolioAccess
        {
            PortfolioId = Guid.Parse(options.Value.TestAuthUserA.PortfolioId),
            UserId = options.Value.TestAuthUserA.UserId
        });

        _userPortfolioAccesses.Add(new UserPortfolioAccess
        {
            PortfolioId = Guid.Parse(options.Value.TestAuthUserB.PortfolioId),
            UserId = options.Value.TestAuthUserB.UserId
        });

        await context.UserPortfolioAccess.AddRangeAsync(_userPortfolioAccesses);
    }
}