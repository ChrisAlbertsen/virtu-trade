using System;

namespace Service.Exceptions.PortfolioExceptions;

public class PortfolioLacksCashException(Guid portfolioId) : Exception($"Portfolio lacks cash: {portfolioId}");