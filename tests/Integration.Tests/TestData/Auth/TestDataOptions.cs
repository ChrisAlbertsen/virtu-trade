using System.ComponentModel.DataAnnotations;

namespace Integration.Tests.TestData.Auth;

public class TestAuthUserConfig
{
    [Required] public string UserId { get; set; } = default!;

    [Required] public string PortfolioId { get; set; } = default!;
}

public class TestDataOptions
{
    [Required] public TestAuthUserConfig TestAuthUserAuthenticated { get; set; } = new();

    [Required] public TestAuthUserConfig TestAuthUserUnauthenticated { get; set; } = new();
}