﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;

namespace Api.Controllers;

public class PaperController(IPaperPortfolioService paperPortfolioService) : ControllerBase
{
    [HttpPost("paper/create-portfolio")]
    public async Task<IActionResult> CreatePortfolio()
    {
        var portfolio = await paperPortfolioService.CreatePortfolio();
        return Ok(portfolio);
    }

    [HttpPost("paper/deposit-money")]
    public async Task DepositMoney(Guid portfolioId, decimal moneyToDeposit)
    {
        await paperPortfolioService.DepositMoneyToPortfolio(portfolioId, moneyToDeposit);
    }

    [HttpGet("paper/portfolio/{portfolioId}")]
    public async Task<IActionResult> GetPortfolio(Guid portfolioId)
    {
        var portfolio = await paperPortfolioService.GetPortfolioWithHoldingsAsync(portfolioId);
        return Ok(portfolio);
    }
}