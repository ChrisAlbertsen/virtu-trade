using System;
using Data.Entities;

namespace Data.AuthModels;

public class UserPortfolioAccess
{ 
    public required string UserId { get; set; }
    public required Guid PortfolioId { get; set; }
}