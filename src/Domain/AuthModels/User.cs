using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Data.AuthModels;

public class User : IdentityUser
{
    public ICollection<UserPortfolioAccess> UserPortfolioAccesses { get; set; } = new List<UserPortfolioAccess>();
}