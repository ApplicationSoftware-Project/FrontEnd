using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using receipt_project_front.Models;

namespace receipt_project_front.Services;

/// <summary>
/// 백엔드 Auth API(/api/auth/*)를 호출합니다.
/// AppState.Current.AccessToken에 토큰을 저장하면
/// ApiClient.ForCurrentUser()가 이후 모든 요청에 Bearer를 자동 부착합니다.
/// </summary>
internal static class AuthApi
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    // ── 로그인: POST /api/auth/login ──────────────────
    public static async Task<LoginResult> LoginAsync(
        string email,
        string password,
        CancellationToken ct = default)
    {
        // 로그인은 토큰 없이 호출 (공개 엔드포인트)
        using var http = CreateAnonymousClient();
        var request = new LoginRequest(email, password);

        using var response = await http.PostAsJsonAsync("/api/auth/login", request, JsonOptions, ct);
        var body = await response.Content.ReadAsStringAsync(ct);

        if (response.StatusCode == HttpStatusCode.OK)
        {
            return JsonSerializer.Deserialize<LoginResult>(body, JsonOptions)
                   ?? throw new AuthException("서버 응답을 해석할 수 없습니다.");
        }

        throw response.StatusCode switch
        {
            HttpStatusCode.Unauthorized =>
                new AuthException("이메일 또는 비밀번호가 올바르지 않습니다."),
            _ =>
                new AuthException($"로그인 실패 ({(int)response.StatusCode})")
        };
    }

    // ── 회원가입: POST /api/auth/register ─────────────
    public static async Task<RegisterResult> RegisterAsync(
        string email,
        string password,
        string displayName,
        CancellationToken ct = default)
    {
        using var http = CreateAnonymousClient();
        var request = new RegisterRequest(email, password, displayName);

        using var response = await http.PostAsJsonAsync("/api/auth/register", request, JsonOptions, ct);
        var body = await response.Content.ReadAsStringAsync(ct);

        if (response.StatusCode == HttpStatusCode.Created)
        {
            return JsonSerializer.Deserialize<RegisterResult>(body, JsonOptions)
                   ?? throw new AuthException("서버 응답을 해석할 수 없습니다.");
        }

        // ValidationProblem 에러 메시지 파싱
        var message = ExtractErrorMessage(body)
                      ?? $"회원가입 실패 ({(int)response.StatusCode})";
        throw new AuthException(message);
    }

    // ── 헬퍼 ──────────────────────────────────────────

    /// <summary>
    /// 로그인/회원가입은 토큰이 없으므로 Authorization 헤더 없이 직접 생성합니다.
    /// </summary>
    private static HttpClient CreateAnonymousClient()
    {
        var handler = new HttpClientHandler
        {
            // DEV ONLY: self-signed 인증서 우회 (ApiClient와 동일)
            ServerCertificateCustomValidationCallback = (_, _, _, _) => true
        };
        return new HttpClient(handler)
        {
            BaseAddress = new Uri(ApiConfig.BaseUrl),
            Timeout = TimeSpan.FromSeconds(30)
        };
    }

    /// <summary>
    /// ValidationProblem(400) 응답에서 첫 번째 에러 메시지를 추출합니다.
    /// </summary>
    private static string? ExtractErrorMessage(string body)
    {
        try
        {
            using var doc = JsonDocument.Parse(body);
            // { "errors": { "register": ["이미 사용 중인 이메일입니다."] } }
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
            // { "detail": "..." }
            if (doc.RootElement.TryGetProperty("detail", out var detail))
                return detail.GetString();
        }
        catch (JsonException) { }
        return null;
    }
}

/// <summary>
/// 로그인/회원가입 실패 시 발생하는 예외입니다.
/// </summary>
internal sealed class AuthException : Exception
{
    public AuthException(string message) : base(message) { }
}
