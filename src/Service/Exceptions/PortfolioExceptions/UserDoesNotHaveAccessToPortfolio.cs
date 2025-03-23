using System;

namespace Service.Exceptions.PortfolioExceptions;

public class UserDoesNotHaveAccessToPortfolio(Guid portfolioId, Guid userId)
    : Exception($"user{userId} does not have access to {portfolioId}");