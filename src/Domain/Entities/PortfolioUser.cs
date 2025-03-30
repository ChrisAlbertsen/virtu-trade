using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Data.Entities;

public class PortfolioUser : IdentityUser
{
    public List<Guid> PortfolioIds = new List<Guid>();
}