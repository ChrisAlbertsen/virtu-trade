using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Data.AuthModels;

public class PortfolioUser : IdentityUser
{
    public ICollection<PortfolioUserMapping> PortfolioUserMappings { get; set; } = new List<PortfolioUserMapping>();
}