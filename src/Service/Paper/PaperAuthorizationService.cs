using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Data.AuthModels;
using Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Service.Interfaces;

namespace Service.Paper;

public class PaperAuthorizationService(
    IHttpContextAccessor httpContextAccessor, 
    UserManager<PortfolioUser> userManager, 
    ILogger<PaperAuthorizationService> logger) : IAuthorizationService
{
    private bool _userIsAuthorized;
    
    public string GetClaimUserIdFromHttpContext()
    {
        var userId = httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId is not null) return userId;
        LogRequestDetails("No userId found in claim");
        throw new UnauthorizedAccessException("No UserId found in claim");
    }

    private async Task<PortfolioUser> GetUserByUserId(string userId)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user is not null) return user;
        LogRequestDetails($"No user exists with id: {userId}");
        throw new UnauthorizedAccessException("User does not exist");
    }
    
    private bool IsUserAllowedAccess(PortfolioUser user, Guid portfolioId)
    {
        return user.PortfolioUserMappings.Contains(portfolioId);
    }

    public async Task VerifyUserHasAccessToPortfolio(Guid portfolioId)
    {
        if(_userIsAuthorized) return;
        
        var userId = GetClaimUserIdFromHttpContext();
        var user = await GetUserByUserId(userId);
        
        if (IsUserAllowedAccess(user, portfolioId))
        {
            _userIsAuthorized = true;
            return;
        }
        
        LogRequestDetails($"User: {userId} is not allowed access to portfolio:{portfolioId}", LogLevel.Warning);
        throw new UnauthorizedAccessException($"User does not have access to portfolio {portfolioId}");
    }

    public async Task<bool> AddPortfolioToPortfolioUser(Guid portfolioId)
    {
        var userId = GetClaimUserIdFromHttpContext();
        var user = await GetUserByUserId(userId);
        user.PortfolioIds.Add(portfolioId);
        var result = await userManager.UpdateAsync(user);
        return result.Succeeded;
    }
    
    //TODO: Configure logging solution to make the below obsolete
    private void LogRequestDetails(string message, LogLevel logLevel = LogLevel.Error)
    {
        var request = httpContextAccessor.HttpContext?.Request;
        var ipAddress = httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString();
        var userAgent = request?.Headers["User-Agent"].ToString();

        if (logLevel == LogLevel.Warning)
        {
            logger.LogWarning(
                "{Message}. Request Information: Path: {RequestPath}, Method: {RequestMethod}, IP Address: {IpAddress}, User-Agent: {UserAgent}",
                message,
                request?.Path,
                request?.Method,
                ipAddress,
                userAgent);
        }
        else
        {
            logger.LogError(
                "{Message}. Request Information: Path: {RequestPath}, Method: {RequestMethod}, IP Address: {IpAddress}, User-Agent: {UserAgent}",
                message,
                request?.Path,
                request?.Method,
                ipAddress,
                userAgent);
        }
    }

}