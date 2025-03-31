using System.Collections.Generic;
using Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace Data.Users;

public class PaperPortfolioUser : IdentityUser
{
    public ICollection<Portfolio> Portfolios { get; set; } = new List<Portfolio>();
}