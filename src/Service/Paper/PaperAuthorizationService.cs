using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Data.AuthModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Persistence;
using Service.Interfaces;

namespace Service.Paper;

public class PaperAuthorizationService(
    IHttpContextAccessor httpContextAccessor,
    AppDbContext dbContext,
    ILogger<PaperAuthorizationService> logger) : IAuthorizationService
{
    private string GetClaimUserIdFromHttpContext()
    {
        var userId = httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId is not null) return userId;
        
        LogRequestDetails("No userId found in claim");
        throw new UnauthorizedAccessException("No UserId found in claim");
    }

    public void GiveUserAccessToPortfolio(Guid portfolioId)
    {
        var portfolioUserMapping = new UserPortfolioAccess
            {PortfolioId = portfolioId, UserId = GetClaimUserIdFromHttpContext() };
        dbContext.UserPortfolioAccess.Add(portfolioUserMapping);
    }


    //TODO: Configure logging solution to make the below obsolete
    private void LogRequestDetails(string message, LogLevel logLevel = LogLevel.Error)
    {
        var request = httpContextAccessor.HttpContext?.Request;
        var ipAddress = httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString();
        var userAgent = request?.Headers["User-Agent"].ToString();

        if (logLevel == LogLevel.Warning)
            logger.LogWarning(
                "{Message}. Request Information: Path: {RequestPath}, Method: {RequestMethod}, IP Address: {IpAddress}, User-Agent: {UserAgent}",
                message,
                request?.Path,
                request?.Method,
                ipAddress,
                userAgent);
        else
            logger.LogError(
                "{Message}. Request Information: Path: {RequestPath}, Method: {RequestMethod}, IP Address: {IpAddress}, User-Agent: {UserAgent}",
                message,
                request?.Path,
                request?.Method,
                ipAddress,
                userAgent);
    }
}