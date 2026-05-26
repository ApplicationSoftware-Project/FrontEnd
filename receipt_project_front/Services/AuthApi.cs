using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using receipt_project_front.Models;

namespace receipt_project_front.Services;

/// <summary>
/// 백엔드 Auth API(/api/auth/*)를 호출합니다.
/// </summary>
internal static class AuthApi
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    // ── 로그인: POST /api/auth/login ──────────────────
    public static async Task<LoginResult> LoginAsync(
        string email, string password, CancellationToken ct = default)
    {
        using var http = CreateAnonymousClient();
        using var response = await http.PostAsJsonAsync(
            "/api/auth/login", new LoginRequest(email, password), JsonOptions, ct);
        var body = await response.Content.ReadAsStringAsync(ct);

        if (response.StatusCode == HttpStatusCode.OK)
            return JsonSerializer.Deserialize<LoginResult>(body, JsonOptions)
                   ?? throw new AuthException("서버 응답을 해석할 수 없습니다.");

        throw response.StatusCode switch
        {
            HttpStatusCode.Unauthorized => new AuthException("이메일 또는 비밀번호가 올바르지 않습니다."),
            _ => new AuthException($"로그인 실패 ({(int)response.StatusCode})")
        };
    }

    // ── 내 정보 조회: GET /api/auth/me ───────────────
    public static async Task<MeResult> GetMeAsync(CancellationToken ct = default)
    {
        using var response = await ApiClient.ForCurrentUser().GetAsync("/api/auth/me", ct);
        var body = await response.Content.ReadAsStringAsync(ct);

        if (response.StatusCode == HttpStatusCode.OK)
            return JsonSerializer.Deserialize<MeResult>(body, JsonOptions)
                   ?? throw new AuthException("서버 응답을 해석할 수 없습니다.");

        throw response.StatusCode switch
        {
            HttpStatusCode.Unauthorized => new AuthException("인증 정보가 유효하지 않습니다."),
            _ => new AuthException($"내 정보 조회 실패 ({(int)response.StatusCode})")
        };
    }

    // ── 회원가입: POST /api/auth/register ─────────────
    public static async Task<RegisterResult> RegisterAsync(
        string email, string password, string displayName, CancellationToken ct = default)
    {
        using var http = CreateAnonymousClient();
        using var response = await http.PostAsJsonAsync(
            "/api/auth/register", new RegisterRequest(email, password, displayName), JsonOptions, ct);
        var body = await response.Content.ReadAsStringAsync(ct);

        if (response.StatusCode == HttpStatusCode.Created)
            return JsonSerializer.Deserialize<RegisterResult>(body, JsonOptions)
                   ?? throw new AuthException("서버 응답을 해석할 수 없습니다.");

        throw new AuthException(ExtractErrorMessage(body) ?? $"회원가입 실패 ({(int)response.StatusCode})");
    }

    // ── 프로필 수정: PUT /api/auth/me/profile ─────────
    public static async Task UpdateProfileAsync(
        string? displayName, CancellationToken ct = default)
    {
        var request = new UpdateProfileRequest(displayName, null, null);
        using var response = await ApiClient.ForCurrentUser()
            .PutAsJsonAsync("/api/auth/me/profile", request, JsonOptions, ct);
        var body = await response.Content.ReadAsStringAsync(ct);

        if (response.StatusCode == HttpStatusCode.NoContent) return;

        throw response.StatusCode switch
        {
            HttpStatusCode.Unauthorized => new AuthException("인증이 필요합니다."),
            _ => new AuthException(ExtractErrorMessage(body) ?? $"프로필 수정 실패 ({(int)response.StatusCode})")
        };
    }

    // ── 비밀번호 변경: PUT /api/auth/me/password ──────
    public static async Task ChangePasswordAsync(
        string currentPassword, string newPassword, CancellationToken ct = default)
    {
        var request = new ChangePasswordRequest(currentPassword, newPassword);
        using var response = await ApiClient.ForCurrentUser()
            .PutAsJsonAsync("/api/auth/me/password", request, JsonOptions, ct);
        var body = await response.Content.ReadAsStringAsync(ct);

        if (response.StatusCode == HttpStatusCode.NoContent) return;

        throw response.StatusCode switch
        {
            HttpStatusCode.Unauthorized => new AuthException("인증이 필요합니다."),
            _ => new AuthException(ExtractErrorMessage(body) ?? $"비밀번호 변경 실패 ({(int)response.StatusCode})")
        };
    }

    // ── 알림 설정 조회: GET /api/auth/me/notifications ─
    public static async Task<NotificationResult> GetNotificationsAsync(CancellationToken ct = default)
    {
        using var response = await ApiClient.ForCurrentUser()
            .GetAsync("/api/auth/me/notifications", ct);
        var body = await response.Content.ReadAsStringAsync(ct);

        if (response.StatusCode == HttpStatusCode.OK)
            return JsonSerializer.Deserialize<NotificationResult>(body, JsonOptions)
                   ?? throw new AuthException("서버 응답을 해석할 수 없습니다.");

        throw response.StatusCode switch
        {
            HttpStatusCode.Unauthorized => new AuthException("인증이 필요합니다."),
            _ => new AuthException($"알림 설정 조회 실패 ({(int)response.StatusCode})")
        };
    }

    // ── 알림 설정 변경: PUT /api/auth/me/notifications ─
    public static async Task<NotificationResult> UpdateNotificationsAsync(
        bool emailNotification, bool pushNotification, CancellationToken ct = default)
    {
        var request = new UpdateNotificationRequest(emailNotification, pushNotification);
        using var response = await ApiClient.ForCurrentUser()
            .PutAsJsonAsync("/api/auth/me/notifications", request, JsonOptions, ct);
        var body = await response.Content.ReadAsStringAsync(ct);

        if (response.StatusCode == HttpStatusCode.OK)
            return JsonSerializer.Deserialize<NotificationResult>(body, JsonOptions)
                   ?? throw new AuthException("서버 응답을 해석할 수 없습니다.");

        throw response.StatusCode switch
        {
            HttpStatusCode.Unauthorized => new AuthException("인증이 필요합니다."),
            _ => new AuthException(ExtractErrorMessage(body) ?? $"알림 설정 저장 실패 ({(int)response.StatusCode})")
        };
    }

    // ── 토큰 갱신: POST /api/auth/refresh ─────────────
    public static async Task<RefreshTokenResult> RefreshAsync(
        string refreshToken, CancellationToken ct = default)
    {
        using var http = CreateAnonymousClient();
        using var response = await http.PostAsJsonAsync(
            "/api/auth/refresh", new RefreshTokenRequest(refreshToken), JsonOptions, ct);
        var body = await response.Content.ReadAsStringAsync(ct);

        if (response.StatusCode == HttpStatusCode.OK)
            return JsonSerializer.Deserialize<RefreshTokenResult>(body, JsonOptions)
                   ?? throw new AuthException("서버 응답을 해석할 수 없습니다.");

        throw new AuthException("토큰 갱신 실패. 다시 로그인해 주세요.");
    }

    // ── 로그아웃: POST /api/auth/revoke ──────────────
    public static async Task RevokeAsync(string refreshToken, CancellationToken ct = default)
    {
        try
        {
            using var http = CreateAnonymousClient();
            await http.PostAsJsonAsync(
                "/api/auth/revoke", new RefreshTokenRequest(refreshToken), JsonOptions, ct);
        }
        catch { /* 실패해도 로컬 상태 정리가 우선 */ }
    }

    // ── 헬퍼 ──────────────────────────────────────────
    private static HttpClient CreateAnonymousClient()
    {
        var handler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (_, _, _, _) => true
        };
        return new HttpClient(handler)
        {
            BaseAddress = new Uri(ApiConfig.BaseUrl),
            Timeout = TimeSpan.FromSeconds(30)
        };
    }

    private static string? ExtractErrorMessage(string body)
    {
        try
        {
            using var doc = JsonDocument.Parse(body);
            if (doc.RootElement.TryGetProperty("errors", out var errors) &&
                errors.ValueKind == JsonValueKind.Object)
            {
                foreach (var prop in errors.EnumerateObject())
                {
                    if (prop.Value.ValueKind == JsonValueKind.Array &&
                        prop.Value.GetArrayLength() > 0)
                        return prop.Value[0].GetString();
                }
            }
            if (doc.RootElement.TryGetProperty("detail", out var detail))
                return detail.GetString();
        }
        catch (JsonException) { }
        return null;
    }
}

internal sealed class AuthException : Exception
{
    public AuthException(string message) : base(message) { }
}