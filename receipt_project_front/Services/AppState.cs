using receipt_project_front.Models;

namespace receipt_project_front.Services;

internal sealed class AppState
{
    public static AppState Current { get; } = new();

    public string? AccessToken { get; set; }
    public MeResult? CurrentUser { get; set; }

    public UploadReceiptResult? PendingUpload { get; set; }

    // Local path to the file the user just uploaded — used so EditPage can show
    // the image immediately without re-fetching it from the backend.
    public string? PendingUploadLocalImagePath { get; set; }

    public void Clear()
    {
        AccessToken = null;
        CurrentUser = null;
        PendingUpload = null;
        PendingUploadLocalImagePath = null;
    }
}
