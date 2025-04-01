using System;
using System.Collections.Generic;
using System.Transactions;
using Data.AuthModels;

namespace Data.Entities;

public class Portfolio
{
    public required Guid Id { get; set; }
    public required decimal Cash { get; set; }
    public required decimal ReservedCash { get; set; }
    public ICollection<Holding> Holdings { get; set; } = new List<Holding>();
    public ICollection<Trade> Trades { get; set; } = new List<Trade>();
    public ICollection<PortfolioUserMapping> PortfolioUserMappings { get; set; } = new List<PortfolioUserMapping>();
}