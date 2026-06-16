namespace receipt_project_front.Services;

/// <summary>
/// 액세스 토큰 만료 전에 자동으로 갱신하는 백그라운드 서비스입니다.
/// 로그인 직후 Start(), 로그아웃 시 Stop()을 호출합니다.
/// </summary>
internal static class TokenRefreshService
{
    private static System.Threading.Timer? _timer;
    private static readonly object _lock = new();

    /// <summary>
    /// 토큰 갱신 타이머를 시작합니다.
    /// expiresInSeconds 기준으로 만료 60초 전에 갱신을 시도합니다.
    /// </summary>
    public static void Start(int expiresInSeconds)
    {
        lock (_lock)
        {
            Stop();

            // 최소 30초 뒤, 만료 60초 전에 갱신 시도
            var delayMs = Math.Max(expiresInSeconds - 60, 30) * 1000;
            _timer = new System.Threading.Timer(_ => _ = RefreshAsync(), null, delayMs, Timeout.Infinite);
        }
    }

    /// <summary>
    /// 타이머를 중지합니다. AppState.Clear() 내부에서도 호출됩니다.
    /// </summary>
    public static void Stop()
    {
        lock (_lock)
        {
            _timer?.Dispose();
            _timer = null;
        }
    }

    private static async Task RefreshAsync()
    {
        var refreshToken = AppState.Current.RefreshToken;
        if (string.IsNullOrEmpty(refreshToken)) return;

        try
        {
            var result = await AuthApi.RefreshAsync(refreshToken);

            AppState.Current.AccessToken = result.AccessToken;
            AppState.Current.RefreshToken = result.RefreshToken;
            AppState.Current.AccessTokenExpiresAt =
                DateTimeOffset.UtcNow.AddSeconds(result.ExpiresIn);

            // 다음 갱신 예약
            Start(result.ExpiresIn);
        }
        catch
        {
            // 갱신 실패 시 타이머 중지 — 사용자는 다음 API 호출 시 401을 받고 재로그인하게 됨
            Stop();
        }
    }
}