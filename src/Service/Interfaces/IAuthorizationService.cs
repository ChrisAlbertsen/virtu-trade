using System;
using System.Threading.Tasks;

namespace Service.Interfaces;

public interface IAuthorizationService
{
    string GetClaimUserIdFromHttpContext();
    void GiveUserAccessToPortfolio(Guid portfolioId);
    Task VerifyUserHasAccessToPortfolio(Guid portfolioId);
}