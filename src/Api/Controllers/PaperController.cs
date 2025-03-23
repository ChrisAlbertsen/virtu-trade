using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PaperController(IPaperPortfolioService paperPortfolioService) : ControllerBase
{
    [HttpPost("create-portfolio")]
    public async Task<IActionResult> CreatePortfolio()
    {
        var portfolio = await paperPortfolioService.CreatePortfolio();
        return Ok(portfolio);
    }

    [HttpPost("deposit-money")]
    public async Task DepositMoney(Guid portfolioId, decimal moneyToDeposit)
    {
        await paperPortfolioService.DepositMoneyToPortfolio(portfolioId, moneyToDeposit);
    }

    [HttpGet("portfolio/{portfolioId}")]
    public async Task<IActionResult> GetPortfolio(Guid portfolioId)
    {
        var portfolio = await paperPortfolioService.GetPortfolioWithHoldingsAsync(portfolioId);
        return Ok(portfolio);
    }
}