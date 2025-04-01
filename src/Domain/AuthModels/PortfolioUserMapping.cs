using System;
using Data.Entities;

namespace Data.AuthModels;

public class PortfolioUserMapping
{
    public Guid PortfolioUserId { get; set; }
    public PortfolioUser PortfolioUser { get; set; }
    
    public Guid PortfolioId { get; set; }
    public Portfolio Portfolio { get; set; }
}