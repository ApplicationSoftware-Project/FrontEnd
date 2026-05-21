namespace receipt_project_front.Models;

// Mirrors backend records in App.Features.Auth.AuthContracts.

public record RegisterRequest(string Email, string Password, string DisplayName);
public record RegisterResult(Guid UserId, string Email, string DisplayName, DateTimeOffset CreatedAt);

public record LoginRequest(string Email, string Password);

// 백엔드 LoginResult와 완전히 일치 (UserId, CreatedAt은 /me에서 별도 조회)
public record LoginResult(
    string AccessToken,
    string RefreshToken,
    string TokenType,
    int ExpiresIn,
    string Email,
    string DisplayName,
    string Role);

// 백엔드 MeResult와 완전히 일치 (PhoneNumber, ProfileImageUrl, 알림 설정 추가)
public record MeResult(
    Guid UserId,
    string Email,
    string DisplayName,
    string Role,
    string? PhoneNumber,
    string? ProfileImageUrl,
    bool EmailNotification,
    bool PushNotification,
    DateTimeOffset CreatedAt,
    DateTimeOffset? LastLoginAt);

// ── 프로필 수정 ───────────────────────────────────
public record UpdateProfileRequest(
    string? DisplayName,
    string? PhoneNumber,
    string? ProfileImageUrl);

// ── 비밀번호 변경 ─────────────────────────────────
public record ChangePasswordRequest(string CurrentPassword, string NewPassword);

// ── 알림 설정 ─────────────────────────────────────
public record UpdateNotificationRequest(bool EmailNotification, bool PushNotification);
public record NotificationResult(bool EmailNotification, bool PushNotification);

// ── 리프레시 토큰 ─────────────────────────────────
public record RefreshTokenRequest(string RefreshToken);
public record RefreshTokenResult(string AccessToken, string RefreshToken, int ExpiresIn);