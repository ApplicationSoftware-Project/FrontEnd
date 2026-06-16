using receipt_project_front.Models;

namespace receipt_project_front.Services;

internal sealed class AppState
{
    public static AppState Current { get; } = new();

    public string? AccessToken { get; set; }
    public string? RefreshToken { get; set; }
    public DateTimeOffset? AccessTokenExpiresAt { get; set; }  // 토큰 만료 시점 추적
    public MeResult? CurrentUser { get; set; }

    public UploadReceiptResult? PendingUpload { get; set; }
    public string? PendingUploadLocalImagePath { get; set; }

    public void Clear()
    {
        TokenRefreshService.Stop();
        AccessToken = null;
        RefreshToken = null;
        AccessTokenExpiresAt = null;
        CurrentUser = null;
        PendingUpload = null;
        PendingUploadLocalImagePath = null;
    }
}