using System;

namespace Data.Exceptions;

public class PortfolioNotFoundException : Exception
{
    public PortfolioNotFoundException(Guid portfolioId)
        : base ("Portfolio not found: " + portfolioId)
    {
        
    }

}

public class PortfolioLacksCash : Exception
{
    public PortfolioLacksCash(Guid portfolioId) 
        : base ("Portfolio lacks cash: " + portfolioId) 
    {
        
    }
}