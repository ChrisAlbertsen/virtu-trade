using JetBrains.Annotations;
using Moq;
using Persistence;
using Service.Paper;

namespace Integration.Tests.Service.Paper;

[TestSubject(typeof(PaperPortfolioService))]
public class PaperPortfolioServiceTest
{
    private readonly Mock<ApplicationDatabaseContext> _dbContext;
    private readonly PaperPortfolioService _paperPortfolioService;

    public PaperPortfolioServiceTest()
    {
        _dbContext = new Mock<ApplicationDatabaseContext>();
        _paperPortfolioService = new PaperPortfolioService(_dbContext.Object);
    }

    //TODO: write tests for methods when TestContainer is implemented
}