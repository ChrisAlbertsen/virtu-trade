using System;

namespace Data.Entities;

public class Holding
{
    public required Guid Id { get; set; }
    public required Guid PortfolioId { get; set; }
    public required string Symbol { get; set; }
    public required decimal Quantity { get; set; }
    public required decimal AveragePurchasePrice { get; set; }
    
    public Portfolio Portfolio { get; set; }
}