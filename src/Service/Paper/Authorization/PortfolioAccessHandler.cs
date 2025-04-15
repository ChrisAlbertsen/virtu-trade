using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Persistence;

namespace Service.Paper.Authorization;

public class PortfolioAccessHandler(
    AppDbContext dbContext,
    IHttpContextAccessor httpContextAccessor,
    ILogger<PortfolioAccessHandler> logger)
    : AuthorizationHandler<PortfolioAccessRequirement, Guid>
{
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PortfolioAccessRequirement requirement,
        Guid portfolioId)
    {
        var userId = httpContextAccessor.HttpContext?.User?
            .FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            logger.LogWarning("No user ID in claims.");
            return;
        }

        var hasAccess = await dbContext.UserPortfolioAccess
            .AnyAsync(upa => upa.UserId == userId && upa.PortfolioId == portfolioId);

        if (hasAccess)
        {
            context.Succeed(requirement);
        }
        else
        {
            logger.LogWarning("User {UserId} denied access to portfolio {PortfolioId}", userId, portfolioId);
        }
    }
}