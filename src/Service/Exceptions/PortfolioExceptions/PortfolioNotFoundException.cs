using System;

namespace Service.Exceptions.PortfolioExceptions;

public class PortfolioNotFoundException : Exception
{
    public PortfolioNotFoundException(Guid portfolioId)
        : base("Portfolio not found: " + portfolioId)
    {
    }
}