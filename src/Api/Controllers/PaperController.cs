using System;
using System.Threading.Tasks;
using Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;

namespace Api.Controllers;

public class PaperController(IPaperPortfolioService paperPortfolioService) : ControllerBase
{
    [HttpPost("paper/create-portfolio")]
    public async Task<Portfolio> CreatePortfolio()
    {
        var portfolio = await paperPortfolioService.CreatePortfolio();
        return portfolio;
    }

    [HttpPost("paper/deposit-money")]
    public async Task DepositMoney(Guid portfolioId, decimal moneyToDeposit)
    {
        await paperPortfolioService.DepositMoneyToPortfolio(portfolioId, moneyToDeposit);
    }
}