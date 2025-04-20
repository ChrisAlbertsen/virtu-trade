using System;

namespace Service.Exceptions.PortfolioExceptions;

public class PortfolioCreationFailedException(Guid portfolioId)
    : Exception($"Portfolio creation failed for portfolio: {portfolioId}");