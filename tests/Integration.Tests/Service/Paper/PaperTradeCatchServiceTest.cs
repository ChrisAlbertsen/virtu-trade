using JetBrains.Annotations;
using Moq;
using Persistence;
using Service.Interfaces;
using Service.Paper;

namespace Integration.Tests.Service.Paper;

[TestSubject(typeof(PaperPortfolioService))]
public class PaperTradeCatchServiceTest
{
    private readonly Mock<AppDbContext> _dbContext;
    private readonly IPaperTradeCatchService _paperTradeCatchService;
    private readonly Mock<IPaperPortfolioService> _portfolioService;

    public PaperTradeCatchServiceTest(Mock<IPaperPortfolioService> portfolioService,
        Mock<AppDbContext> dbContext)
    {
        _portfolioService = portfolioService;
        _dbContext = dbContext;
        _paperTradeCatchService = new PaperTradeCatchService(portfolioService.Object, dbContext.Object);
    }

    //TODO: Write integration tests when TestContainer is implemented
}