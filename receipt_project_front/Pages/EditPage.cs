using System.Globalization;
using receipt_project_front.Models;
using receipt_project_front.Services;
using receipt_project_front.UI;

namespace receipt_project_front.Pages;

public partial class EditPage : UserControl, IRefreshablePage
{
    public EditPage()
    {
        InitializeComponent();
    }

    public void OnNavigatedTo()
    {
        var pending = AppState.Current.PendingUpload;
        if (pending is null)
        {
            ClearForm();
            return;
        }

        LoadFromUpload(pending);
    }

    private void LoadFromUpload(UploadReceiptResult upload)
    {
        try
        {
            photoBox.Image?.Dispose();
            if (File.Exists(upload.ImagePath))
                photoBox.Image = Image.FromFile(upload.ImagePath);
        }
        catch
        {
            photoBox.Image = null;
        }

        storeTextBox.Text = upload.Ocr.StoreName ?? string.Empty;
        amountTextBox.Text = upload.Ocr.Amount?.ToString("0.##", CultureInfo.InvariantCulture) ?? string.Empty;
        purchasedAtPicker.Value = (upload.Ocr.PurchasedAt ?? DateTimeOffset.Now).LocalDateTime;
        categoryTextBox.Text = upload.AiSuggestedCategory ?? string.Empty;
        aiSuggestionLabel.Text = string.IsNullOrEmpty(upload.AiSuggestedCategory)
            ? "AI 추천: 없음"
            : $"AI 추천: {upload.AiSuggestedCategory}";
    }

    private void ClearForm()
    {
        photoBox.Image?.Dispose();
        photoBox.Image = null;
        storeTextBox.Text = string.Empty;
        amountTextBox.Text = string.Empty;
        purchasedAtPicker.Value = DateTime.Now;
        categoryTextBox.Text = string.Empty;
        aiSuggestionLabel.Text = "업로드된 영수증이 없습니다";
    }

    private void SubmitButton_Click(object? sender, EventArgs e)
    {
        // TODO: POST /api/receipts/{id}/confirm with { FinalCategory = categoryTextBox.Text }
        MessageBox.Show("저장되었습니다. (스캐폴딩)", "No More Receipts",
            MessageBoxButtons.OK, MessageBoxIcon.Information);
    }
}
