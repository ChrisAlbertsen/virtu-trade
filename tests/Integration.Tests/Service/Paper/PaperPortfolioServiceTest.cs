using JetBrains.Annotations;
using Moq;
using Persistence;
using Service.Interfaces;
using Service.Paper;

namespace Integration.Tests.Service.Paper;

[TestSubject(typeof(PaperPortfolioService))]
public class PaperPortfolioServiceTest
{
    private readonly PaperPortfolioService _paperPortfolioService;

    public PaperPortfolioServiceTest()
    {
        Mock<AppDbContext> dbContext = new();
        Mock<IAuthorizationService> authorizationService = new();
        _paperPortfolioService = new PaperPortfolioService(dbContext.Object, authorizationService.Object);
    }

    //TODO: write tests for methods when TestContainer is implemented
}