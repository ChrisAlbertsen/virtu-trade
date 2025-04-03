using System;
using Data.Entities;

namespace Data.AuthModels;

public class PortfolioUserMapping
{
    public required Guid Id { get; set; }
    public required string PortfolioUserId { get; set; }
    public PortfolioUser? PortfolioUser { get; set; }

    public required Guid PortfolioId { get; set; }
    public Portfolio? Portfolio { get; set; }
}