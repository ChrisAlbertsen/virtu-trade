using System;

namespace Service.Exceptions.PortfolioExceptions;

public class PortfolioLacksCashException : Exception
{
    public PortfolioLacksCashException(Guid portfolioId)
        : base("Portfolio lacks cash: " + portfolioId)
    {
    }
}