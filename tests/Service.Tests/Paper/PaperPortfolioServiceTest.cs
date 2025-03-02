using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Data.Entities;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using Moq;
using Newtonsoft.Json;
using Persistence;
using Service.Paper;

namespace Service.Tests.Paper;

[TestSubject(typeof(PaperPortfolioService))]
public class PaperPortfolioServiceTest
{
    private readonly string DUMMYPORTFOLIOPATH = "tests/Service.Tests/Paper/DummyData/DummyPortfolios.json";
    private readonly Mock<ApplicationDatabaseContext> _dbContext;
    private readonly PaperPortfolioService _paperPortfolioService;
    
    public PaperPortfolioServiceTest()
    {
        _dbContext = new Mock<ApplicationDatabaseContext>();
        _paperPortfolioService = new PaperPortfolioService(_dbContext.Object);
        var portfolios = LoadPortfoliosFromJson(DUMMYPORTFOLIOPATH);
        _dbContext.Setup(db => db.Portfolios).Returns(portfolios);
    }
    
    private static DbSet<Portfolio>? LoadPortfoliosFromJson(string filePath)
    {
        var json = File.ReadAllText(filePath);
        var portfolios = JsonConvert.DeserializeObject<DbSet<Portfolio>>(json);
        if (portfolios == null) throw new TestCanceledException();
        return portfolios;
    }
    
    [Trait("Category", "Unit test")]
    [Fact(DisplayName = "Should return List of Holding")]
    public async Task GetHoldingAsync_ShouldReturnListOfHoldings()
    {
        var portfolioId = It.IsAny<Guid>();
        
        _dbContext.Po
        
        
    }
}