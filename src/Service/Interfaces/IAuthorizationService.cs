using System;
using System.Threading.Tasks;

namespace Service.Interfaces;

public interface IAuthorizationService
{
    string GetClaimUserIdFromHttpContext();
    Task VerifyUserHasAccessToPortfolio(Guid portfolioId);
    Task<bool> AddPortfolioToPortfolioUser(Guid portfolioId);
}