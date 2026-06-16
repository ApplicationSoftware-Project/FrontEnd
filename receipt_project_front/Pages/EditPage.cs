using System.Globalization;
using receipt_project_front.Models;
using receipt_project_front.Services;
using receipt_project_front.UI;

namespace receipt_project_front.Pages;

public partial class EditPage : UserControl, IRefreshablePage
{
    public event EventHandler? SaveCompleted;

    private bool _saving;

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
        // upload.ImagePath is a server-relative path; show the local file the
        // user just uploaded so the photo appears immediately.
        var localPath = AppState.Current.PendingUploadLocalImagePath;
        try
        {
            photoBox.Image?.Dispose();
            photoBox.Image = !string.IsNullOrEmpty(localPath) && File.Exists(localPath)
                ? Image.FromFile(localPath)
                : null;
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

        submitButton.Enabled = true;
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
        submitButton.Enabled = false;
    }

    private async void SubmitButton_Click(object? sender, EventArgs e)
    {
        if (_saving) return;

        var pending = AppState.Current.PendingUpload;
        if (pending is null)
        {
            MessageBox.Show("저장할 영수증이 없습니다.", "No More Receipts",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var category = categoryTextBox.Text.Trim();
        if (string.IsNullOrEmpty(category))
        {
            MessageBox.Show("카테고리를 입력해 주세요.", "No More Receipts",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            categoryTextBox.Focus();
            return;
        }

        // 사용자가 수정한 기초 정보(상호명·금액·날짜)를 파싱한다.
        var storeName = storeTextBox.Text.Trim();

        decimal? amount = null;
        var amountText = amountTextBox.Text.Replace(",", string.Empty).Trim();
        if (!string.IsNullOrEmpty(amountText))
        {
            if (!decimal.TryParse(amountText, NumberStyles.Number, CultureInfo.InvariantCulture, out var parsedAmount))
            {
                MessageBox.Show("금액은 숫자로 입력해 주세요.", "저장 실패",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                amountTextBox.Focus();
                return;
            }
            amount = parsedAmount;
        }

        var purchasedAt = new DateTimeOffset(purchasedAtPicker.Value);

        SetSaving(true);
        try
        {
            // 1) 수정한 기초 정보 저장 (빈 칸은 null로 보내 변경하지 않음).
            await ReceiptApi.UpdateAsync(pending.ReceiptId, new UpdateReceiptRequest(
                StoreName: string.IsNullOrEmpty(storeName) ? null : storeName,
                Amount: amount,
                PurchasedAt: purchasedAt));

            // 2) 카테고리 확정.
            await ReceiptApi.ConfirmCategoryAsync(pending.ReceiptId, category);

            AppState.Current.PendingUpload = null;
            AppState.Current.PendingUploadLocalImagePath = null;

            SaveCompleted?.Invoke(this, EventArgs.Empty);
        }
        catch (ApiException apiEx)
        {
            MessageBox.Show(apiEx.Message, "저장 실패",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        catch (HttpRequestException ex)
        {
            MessageBox.Show($"백엔드에 연결할 수 없습니다.\n{ex.Message}", "저장 실패",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"저장 실패: {ex.Message}", "저장 실패",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        finally
        {
            SetSaving(false);
        }
    }

    private void SetSaving(bool saving)
    {
        _saving = saving;
        submitButton.Enabled = !saving;
        submitButton.Text = saving ? "저장 중..." : "저장";
        categoryTextBox.Enabled = !saving;
        storeTextBox.Enabled = !saving;
        amountTextBox.Enabled = !saving;
        purchasedAtPicker.Enabled = !saving;
    }
}
