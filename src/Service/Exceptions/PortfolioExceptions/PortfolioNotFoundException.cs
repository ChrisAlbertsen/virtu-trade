using System;

namespace Service.Exceptions.PortfolioExceptions;

public class PortfolioNotFoundException(Guid portfolioId) : Exception($"Portfolio not found: {portfolioId}");