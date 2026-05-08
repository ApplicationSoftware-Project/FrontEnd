using receipt_project_front.Services;
using receipt_project_front.UI;

namespace receipt_project_front.Pages;

public partial class UploadPage : UserControl
{
    public event EventHandler? UploadCompleted;

    private bool _busy;

    public UploadPage()
    {
        InitializeComponent();
        dropZone.Click += (_, _) => PickAndUpload();
        dropArrowLabel.Click += (_, _) => PickAndUpload();
        dropHintLabel.Click += (_, _) => PickAndUpload();
    }

    private async void PickAndUpload()
    {
        if (_busy) return;

        string filePath;
        using (var dialog = new OpenFileDialog
        {
            Filter = "이미지 파일 (*.jpg;*.jpeg;*.png;*.webp)|*.jpg;*.jpeg;*.png;*.webp",
            Title = "영수증 이미지 선택"
        })
        {
            if (dialog.ShowDialog() != DialogResult.OK) return;
            filePath = dialog.FileName;
        }

        SetBusy(true);
        try
        {
            var result = await ReceiptApi.UploadAsync(filePath);

            AppState.Current.PendingUpload = result;
            AppState.Current.PendingUploadLocalImagePath = filePath;

            UploadCompleted?.Invoke(this, EventArgs.Empty);
        }
        catch (ApiException apiEx)
        {
            ShowError(apiEx.Message);
        }
        catch (HttpRequestException ex)
        {
            ShowError($"백엔드에 연결할 수 없습니다.\n{ex.Message}");
        }
        catch (Exception ex)
        {
            ShowError($"업로드 실패: {ex.Message}");
        }
        finally
        {
            SetBusy(false);
        }
    }

    private void SetBusy(bool busy)
    {
        _busy = busy;
        dropZone.Enabled = !busy;
        if (busy)
        {
            dropArrowLabel.Text = "⌛";
            dropArrowLabel.ForeColor = AppTheme.TextSecondary;
            dropHintLabel.Text = "업로드 중...";
        }
        else
        {
            dropArrowLabel.Text = "↑";
            dropArrowLabel.ForeColor = AppTheme.Accent;
            dropHintLabel.Text = "클릭하여 영수증 업로드";
        }
    }

    private void ShowError(string message)
    {
        MessageBox.Show(message, "업로드 실패",
            MessageBoxButtons.OK, MessageBoxIcon.Warning);
    }
}
