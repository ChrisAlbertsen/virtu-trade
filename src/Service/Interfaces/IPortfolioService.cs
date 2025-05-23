﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Entities;

namespace Service.Interfaces;

public interface IPortfolioService
{
    Task<Portfolio> CreatePortfolio();
    Task DepositMoneyToPortfolio(Guid portfolioId, decimal moneyToDeposit);
    Task<Holding?> FindHoldingWithSymbol(Guid portfolioId, string symbol);
    Task<List<Holding>> GetHoldingsAsync(Guid portfolioId);
    Task CheckAndReserveCashAmountAsync(Guid portfolioId, decimal cashToReserve);
    Task PayReservedCash(Guid portfolioId, decimal cashToPay);
    Task UnreserveCash(Guid portfolioId, decimal cashToUnreserve);
    Task<Portfolio?> GetPortfolioAsync(Guid portfolioId);
    Task<Portfolio?> GetPortfolioWithHoldingsAsync(Guid portfolioId);
}