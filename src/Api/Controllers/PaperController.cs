using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using IAuthorizationService = Microsoft.AspNetCore.Authorization.IAuthorizationService;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PaperController(IPortfolioService portfolioService, IAuthorizationService authorizationService)
    : ControllerBase
{
    [HttpPost("create-portfolio")]
    public async Task<IActionResult> CreatePortfolio()
    {
        var portfolio = await portfolioService.CreatePortfolio();
        return Ok(portfolio);
    }

    [HttpPost("deposit-money")]
    public async Task<IActionResult> DepositMoney(Guid portfolioId, decimal moneyToDeposit)
    {
        var result = await authorizationService.AuthorizeAsync(User, portfolioId, "canAccessPortfolio");
        if (!result.Succeeded) return Forbid();
        await portfolioService.DepositMoneyToPortfolio(portfolioId, moneyToDeposit);
        return Ok();
    }

    [HttpGet("portfolio/{portfolioId}")]
    public async Task<IActionResult> GetPortfolio(Guid portfolioId)
    {
        var result = await authorizationService.AuthorizeAsync(User, portfolioId, "canAccessPortfolio");
        if (!result.Succeeded) return Forbid();
        var portfolio = await portfolioService.GetPortfolioWithHoldingsAsync(portfolioId);
        return Ok(portfolio);
    }
}