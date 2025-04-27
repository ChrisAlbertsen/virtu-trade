using System.ComponentModel.DataAnnotations;

namespace Integration.Tests.TestData.Auth;

public class TestAuthUserConfig
{
    [Required] public string UserId { get; set; } = default!;

    [Required] public string PortfolioId { get; set; } = default!;
}

public class TestAuthOptions
{
    [Required] public TestAuthUserConfig TestAuthUserA { get; set; } = new();

    [Required] public TestAuthUserConfig TestAuthUserB { get; set; } = new();
}