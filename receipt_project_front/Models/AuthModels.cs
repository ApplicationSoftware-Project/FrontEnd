namespace receipt_project_front.Models;

// Mirrors backend records in App.Features.Auth.AuthContracts.

public record RegisterRequest(string Email, string Password, string DisplayName);
public record RegisterResult(Guid UserId, string Email, string DisplayName, DateTimeOffset CreatedAt);

public record LoginRequest(string Email, string Password);

// ※ 이전 PR에서 RefreshToken, UserId, CreatedAt 누락 — 백엔드 LoginResult와 완전히 맞춤
public record LoginResult(
    string AccessToken,
    string RefreshToken,   // 추가: 토큰 갱신용
    string TokenType,
    int ExpiresIn,
    string Email,
    string DisplayName,
    string Role,
    Guid UserId,           // 추가: AppState.CurrentUser 구성용
    DateTimeOffset CreatedAt); // 추가: AppState.CurrentUser 구성용

public record MeResult(
    Guid UserId,
    string Email,
    string DisplayName,
    string Role,
    DateTimeOffset CreatedAt,
    DateTimeOffset? LastLoginAt);