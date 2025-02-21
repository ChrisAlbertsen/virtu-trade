using System;
using System.Threading.Tasks;
using Data.Entities;

namespace Service.Interfaces;

public interface IPaperBrokerOrderService
{
    Task<Portfolio> CreatePortfolio();
    Task DepositMoneyToPortfolio(Guid portfolioId, decimal moneyToDeposit);
}