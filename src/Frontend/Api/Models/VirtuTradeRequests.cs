namespace Frontend.Api.Models;

public record RegisterRequest(string? Email, string? Password);

public record LoginRequest(string? Email, string? Password, string? TwoFactorCode, string? TwoFactorRecoveryCode);

public record RefreshRequest(string? RefreshToken);

public record AccessTokenResponse(string? TokenType, string? AccessToken, long ExpiresIn, string? RefreshToken);

public record ForgotPasswordRequest(string? Email);

public record ResendConfirmationEmailRequest(string? Email);

public record ResetPasswordRequest(string? Email, string? ResetCode, string? NewPassword);

public record TwoFactorRequest(
    bool? Enable,
    string? TwoFactorCode,
    bool ResetSharedKey,
    bool ResetRecoveryCodes,
    bool ForgetMachine);

public record TwoFactorResponse(
    string? SharedKey,
    int RecoveryCodesLeft,
    IEnumerable<string>? RecoveryCodes,
    bool IsTwoFactorEnabled,
    bool IsMachineRemembered);

public record InfoRequest(string? NewEmail, string? NewPassword, string? OldPassword);

public record InfoResponse(string? Email, bool IsEmailConfirmed);

public record MarketOrderParams(Guid PortfolioId, string? Symbol, double Quantity);