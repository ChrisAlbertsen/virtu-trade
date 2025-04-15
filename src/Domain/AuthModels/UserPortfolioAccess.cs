using System;

namespace Data.AuthModels;

public class UserPortfolioAccess
{
    public required string UserId { get; set; }
    public required Guid PortfolioId { get; set; }
}