using receipt_project_front.Services;
using receipt_project_front.UI;

namespace receipt_project_front.Pages;

public partial class SettingsPage : UserControl, IRefreshablePage
{
    public event EventHandler? SignOutRequested;

    private bool _savingProfile;
    private bool _changingPassword;

    public SettingsPage()
    {
        InitializeComponent();
    }

    // ── 페이지 진입 ───────────────────────────────────
    public void OnNavigatedTo()
    {
        LoadUserInfo();
        devTokenTextBox.Text = AppState.Current.AccessToken ?? string.Empty;
        UpdateTokenStatus();
    }

    private void LoadUserInfo()
    {
        var user = AppState.Current.CurrentUser;
        if (user is null) return;

        profileNameLabel.Text = user.DisplayName;
        profileEmailLabel.Text = user.Email;
        nameTextBox.Text = user.DisplayName;
        emailTextBox.Text = user.Email;

        avatarInitialLabel.Text = user.DisplayName.Length > 0
            ? user.DisplayName[0].ToString().ToUpper()
            : "U";
    }

    // ── 프로필 저장 ───────────────────────────────────
    private async void SaveProfileButton_Click(object? sender, EventArgs e)
    {
        if (_savingProfile) return;

        var displayName = nameTextBox.Text.Trim();
        if (string.IsNullOrWhiteSpace(displayName))
        {
            SetProfileStatus("이름을 입력해 주세요.", isError: true);
            nameTextBox.Focus();
            return;
        }

        SetProfileBusy(true);
        try
        {
            await AuthApi.UpdateProfileAsync(displayName);

            if (AppState.Current.CurrentUser is not null)
                AppState.Current.CurrentUser = AppState.Current.CurrentUser
                    with
                { DisplayName = displayName };

            profileNameLabel.Text = displayName;
            avatarInitialLabel.Text = displayName[0].ToString().ToUpper();
            SetProfileStatus("저장되었습니다.", isError: false);
        }
        catch (AuthException ex)
        {
            SetProfileStatus(ex.Message, isError: true);
        }
        catch (HttpRequestException)
        {
            SetProfileStatus("서버에 연결할 수 없습니다.", isError: true);
        }
        finally
        {
            SetProfileBusy(false);
        }
    }

    private void SetProfileBusy(bool busy)
    {
        _savingProfile = busy;
        saveProfileButton.Enabled = !busy;
        saveProfileButton.Text = busy ? "저장 중..." : "프로필 저장";
    }

    private void SetProfileStatus(string message, bool isError)
    {
        profileStatusLabel.Text = message;
        profileStatusLabel.ForeColor = isError ? AppTheme.Danger : AppTheme.Success;
    }

    // ── 비밀번호 변경 ─────────────────────────────────
    private async void ChangePasswordButton_Click(object? sender, EventArgs e)
    {
        if (_changingPassword) return;

        var current = currentPasswordBox.Text;
        var newPw = newPasswordBox.Text;
        var confirm = confirmPasswordBox.Text;

        if (string.IsNullOrWhiteSpace(current))
        {
            SetPasswordStatus("현재 비밀번호를 입력해 주세요.", isError: true);
            currentPasswordBox.Focus();
            return;
        }
        if (newPw.Length < 6)
        {
            SetPasswordStatus("새 비밀번호는 6자 이상이어야 합니다.", isError: true);
            newPasswordBox.Focus();
            return;
        }
        if (newPw != confirm)
        {
            SetPasswordStatus("새 비밀번호가 일치하지 않습니다.", isError: true);
            confirmPasswordBox.Focus();
            return;
        }

        SetPasswordBusy(true);
        try
        {
            await AuthApi.ChangePasswordAsync(current, newPw);

            currentPasswordBox.Text = string.Empty;
            newPasswordBox.Text = string.Empty;
            confirmPasswordBox.Text = string.Empty;
            SetPasswordStatus("비밀번호가 변경되었습니다.", isError: false);
        }
        catch (AuthException ex)
        {
            SetPasswordStatus(ex.Message, isError: true);
        }
        catch (HttpRequestException)
        {
            SetPasswordStatus("서버에 연결할 수 없습니다.", isError: true);
        }
        finally
        {
            SetPasswordBusy(false);
        }
    }

    private void SetPasswordBusy(bool busy)
    {
        _changingPassword = busy;
        changePasswordButton.Enabled = !busy;
        changePasswordButton.Text = busy ? "변경 중..." : "비밀번호 변경";
        currentPasswordBox.Enabled = !busy;
        newPasswordBox.Enabled = !busy;
        confirmPasswordBox.Enabled = !busy;
    }

    private void SetPasswordStatus(string message, bool isError)
    {
        passwordStatusLabel.Text = message;
        passwordStatusLabel.ForeColor = isError ? AppTheme.Danger : AppTheme.Success;
    }

    // ── 개발자 토큰 (임시) ────────────────────────────
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
            tokenStatusLabel.ForeColor = AppTheme.TextMuted;
        }
        else
        {
            var preview = token.Length > 24
                ? token.Substring(0, 12) + "…" + token.Substring(token.Length - 8)
                : token;
            tokenStatusLabel.Text = $"저장됨 ({preview})";
            tokenStatusLabel.ForeColor = AppTheme.Success;
        }
    }

    // ── 로그아웃 ──────────────────────────────────────
    private async void SignOutButton_Click(object? sender, EventArgs e)
    {
        var refreshToken = AppState.Current.RefreshToken;
        if (!string.IsNullOrEmpty(refreshToken))
            await AuthApi.RevokeAsync(refreshToken);

        SignOutRequested?.Invoke(this, EventArgs.Empty);
    }
}