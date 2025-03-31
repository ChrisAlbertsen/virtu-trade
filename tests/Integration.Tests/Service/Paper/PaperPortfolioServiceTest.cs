using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Moq;
using Persistence;
using Service.Paper;

namespace Integration.Tests.Service.Paper;

[TestSubject(typeof(PaperPortfolioService))]
public class PaperPortfolioServiceTest
{
    private readonly PaperPortfolioService _paperPortfolioService;
    
    public PaperPortfolioServiceTest()
    {
        Mock<AppDbContext> dbContext = new();
        Mock<IHttpContextAccessor> httpContextAccessor = new();
        _paperPortfolioService = new PaperPortfolioService(dbContext.Object, httpContextAccessor.Object);
    }

    //TODO: write tests for methods when TestContainer is implemented
}