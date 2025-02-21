using System;
using System.Threading.Tasks;
using Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
namespace Api.Controllers;

public class PaperController(IPaperBrokerOrderService paperBrokerOrderService) : ControllerBase
{
    [HttpPost("paper/create-portfolio")]
    public async Task<Portfolio> CreatePortfolio()
    {
        var portfolio = await paperBrokerOrderService.CreatePortfolio();
        return portfolio;
    }

    [HttpPost("paper/deposit-money")]
    public async Task DepositMoney(Guid portfolioId, decimal moneyToDeposit)
    {
        await paperBrokerOrderService.DepositMoneyToPortfolio(portfolioId, moneyToDeposit);
    }
}