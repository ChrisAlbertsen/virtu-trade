using System;
using Data.Entities;

namespace Data.AuthModels;

public class UserPortfolioAccess
{
    public required Guid Id { get; set; }
    public required string UserId { get; set; }
    public required Guid PortfolioId { get; set; }
    
    public User User { get; set; } = null!;
    public Portfolio Portfolio { get; set; } = null!;
}