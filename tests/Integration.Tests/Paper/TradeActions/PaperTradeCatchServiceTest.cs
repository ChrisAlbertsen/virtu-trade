using JetBrains.Annotations;
using Moq;
using Persistence;
using Service.Interfaces;
using Service.Paper;

namespace Integration.Tests.Paper.TradeActions;

[TestSubject(typeof(PaperPortfolioService))]
public class PaperTradeCatchServiceTest
{
    private readonly Mock<AppDbContext> _dbContext;
    private readonly Mock<IPortfolioService> _portfolioService;
    private readonly ITradeCatchService _tradeCatchService;

    public PaperTradeCatchServiceTest(Mock<IPortfolioService> portfolioService,
        Mock<AppDbContext> dbContext)
    {
        _portfolioService = portfolioService;
        _dbContext = dbContext;
        _tradeCatchService = new PaperTradeCatchService(portfolioService.Object, dbContext.Object);
    }

    //TODO: Write integration tests when TestContainer is implemented
}