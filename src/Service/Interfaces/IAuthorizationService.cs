using System;
using System.Threading.Tasks;

namespace Service.Interfaces;

public interface IAuthorizationService
{
    void GiveUserAccessToPortfolio(Guid portfolioId);
}