using receipt_project_front.Services;

namespace receipt_project_front.Pages;

public partial class SettingsPage : UserControl, IRefreshablePage
{
    public event EventHandler? SignOutRequested;

    public SettingsPage()
    {
        InitializeComponent();
    }

    public void OnNavigatedTo()
    {
        devTokenTextBox.Text = AppState.Current.AccessToken ?? string.Empty;
        UpdateTokenStatus();
    }

    private void SaveTokenButton_Click(object? sender, EventArgs e)
    {
        var token = devTokenTextBox.Text.Trim();
        AppState.Current.AccessToken = string.IsNullOrEmpty(token) ? null : token;
        UpdateTokenStatus();
    }

    private void UpdateTokenStatus()
    {
        var token = AppState.Current.AccessToken;
        if (string.IsNullOrEmpty(token))
        {
            tokenStatusLabel.Text = "토큰 없음";
            tokenStatusLabel.ForeColor = UI.AppTheme.TextMuted;
        }
        else
        {
            var preview = token.Length > 24
                ? token.Substring(0, 12) + "…" + token.Substring(token.Length - 8)
                : token;
            tokenStatusLabel.Text = $"저장됨 ({preview})";
            tokenStatusLabel.ForeColor = UI.AppTheme.Success;
        }
    }

    private void SignOutButton_Click(object? sender, EventArgs e)
    {
        SignOutRequested?.Invoke(this, EventArgs.Empty);
    }
}
