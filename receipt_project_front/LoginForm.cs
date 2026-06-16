using receipt_project_front.Services;
using receipt_project_front.UI;

namespace receipt_project_front;

public partial class LoginForm : Form
{
    private bool _busy;

    public LoginForm()
    {
        InitializeComponent();
        registerLink.LinkClicked += (_, _) => OpenRegisterForm();
        passwordBox.KeyDown += async (_, e) =>
        {
            if (e.KeyCode == Keys.Enter)
                await OnLoginClickAsync();
        };
    }

    private async void LoginButton_Click(object? sender, EventArgs e)
        => await OnLoginClickAsync();

    private async Task OnLoginClickAsync()
    {
        if (_busy) return;

        var email = emailBox.Text.Trim();
        var password = passwordBox.Text;

        if (!email.Contains('@') || string.IsNullOrWhiteSpace(email))
        {
            ShowError("올바른 이메일을 입력하세요.");
            emailBox.Focus();
            return;
        }
        if (string.IsNullOrWhiteSpace(password))
        {
            ShowError("비밀번호를 입력하세요.");
            passwordBox.Focus();
            return;
        }

        SetBusy(true);
        try
        {
            // 1단계: 로그인 → AccessToken, RefreshToken 취득
            var loginResult = await AuthApi.LoginAsync(email, password);

            AppState.Current.AccessToken = loginResult.AccessToken;
            AppState.Current.RefreshToken = loginResult.RefreshToken;
            AppState.Current.AccessTokenExpiresAt =
                DateTimeOffset.UtcNow.AddSeconds(loginResult.ExpiresIn);

            // 2단계: /me 호출 → 전체 사용자 정보 취득
            AppState.Current.CurrentUser = await AuthApi.GetMeAsync();

            // 3단계: 백그라운드 토큰 자동 갱신 시작
            TokenRefreshService.Start(loginResult.ExpiresIn);

            DialogResult = DialogResult.OK;
            Close();
        }
        catch (AuthException ex)
        {
            AppState.Current.Clear();
            ShowError(ex.Message);
            passwordBox.Focus();
            passwordBox.SelectAll();
        }
        catch (HttpRequestException)
        {
            AppState.Current.Clear();
            ShowError("서버에 연결할 수 없습니다. 백엔드가 실행 중인지 확인하세요.");
        }
        catch (Exception ex)
        {
            AppState.Current.Clear();
            ShowError($"오류: {ex.Message}");
        }
        finally
        {
            SetBusy(false);
        }
    }

    private void OpenRegisterForm()
    {
        using var registerForm = new RegisterForm();
        if (registerForm.ShowDialog(this) == DialogResult.OK)
        {
            emailBox.Text = registerForm.RegisteredEmail ?? string.Empty;
            passwordBox.Text = string.Empty;
            passwordBox.Focus();

            errorLabel.Text = "회원가입 완료! 비밀번호를 입력하고 로그인하세요.";
            errorLabel.ForeColor = AppTheme.Success;
            errorLabel.Visible = true;
        }
    }

    private void ShowError(string message)
    {
        errorLabel.Text = message;
        errorLabel.ForeColor = AppTheme.Danger;
        errorLabel.Visible = true;
    }

    private void SetBusy(bool busy)
    {
        _busy = busy;
        loginButton.Enabled = !busy;
        loginButton.Text = busy ? "로그인 중..." : "로그인";
        emailBox.Enabled = !busy;
        passwordBox.Enabled = !busy;
        if (busy) errorLabel.Visible = false;
    }

    private void errorLabel_Click(object sender, EventArgs e) { }
}