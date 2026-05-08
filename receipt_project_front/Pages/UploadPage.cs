using receipt_project_front.Models;
using receipt_project_front.Services;
using receipt_project_front.UI;

namespace receipt_project_front.Pages;

public partial class UploadPage : UserControl
{
    public event EventHandler? UploadCompleted;

    public UploadPage()
    {
        InitializeComponent();
        dropZone.Click += (_, _) => PickAndUpload();
        dropArrowLabel.Click += (_, _) => PickAndUpload();
        dropHintLabel.Click += (_, _) => PickAndUpload();
    }

    private void PickAndUpload()
    {
        using var dialog = new OpenFileDialog
        {
            Filter = "이미지 파일 (*.jpg;*.jpeg;*.png;*.webp)|*.jpg;*.jpeg;*.png;*.webp",
            Title = "영수증 이미지 선택"
        };
        if (dialog.ShowDialog() != DialogResult.OK) return;

        // TODO: POST multipart/form-data to /api/receipts/upload, store the parsed
        //       UploadReceiptResult in AppState, then surface UploadCompleted.
        AppState.Current.PendingUpload = new UploadReceiptResult(
            ReceiptId: Guid.NewGuid(),
            ImagePath: dialog.FileName,
            Ocr: new OcrResult(true, "", null, null, "", Array.Empty<string>()),
            AiSuggestedCategory: null,
            AiConfidence: null,
            AiLogId: null,
            Status: ReceiptStatus.Pending,
            Warnings: Array.Empty<string>());

        UploadCompleted?.Invoke(this, EventArgs.Empty);
    }
}
