using receipt_project_front.Services;
using receipt_project_front.UI;

namespace receipt_project_front;

public partial class RegisterForm : Form
{
    
    private bool _busy;

    // 회원가입 성공 시 LoginForm으로 이메일 전달용
    public string? RegisteredEmail { get; private set; }

    public RegisterForm()
    {
        InitializeComponent();

        registerButton.Click += async (_, _) => await OnRegisterClickAsync();
        passwordBox.KeyDown += async (_, e) =>
        {
            if (e.KeyCode == Keys.Enter)
                await OnRegisterClickAsync();
        };
    }

    private async Task OnRegisterClickAsync()
    {
        if (_busy) return;

        var email       = emailBox.Text.Trim();
        var displayName = displayNameBox.Text.Trim();
        var password    = passwordBox.Text;
        var confirm     = confirmBox.Text;

        // 클라이언트 유효성 검사
        if (!email.Contains('@') || string.IsNullOrWhiteSpace(email))
        {
            ShowError("올바른 이메일을 입력하세요.");
            emailBox.Focus();
            return;
        }
        if (string.IsNullOrWhiteSpace(displayName))
        {
            ShowError("닉네임을 입력하세요.");
            displayNameBox.Focus();
            return;
        }
        if (password.Length < 6)
        {
            ShowError("비밀번호는 6자 이상이어야 합니다.");
            passwordBox.Focus();
            return;
        }
        if (password != confirm)
        {
            ShowError("비밀번호가 일치하지 않습니다.");
            confirmBox.Focus();
            return;
        }

        SetBusy(true);
        try
        {
            await AuthApi.RegisterAsync(email, password, displayName);
            RegisteredEmail = email;
            DialogResult = DialogResult.OK;
            Close();
        }
        catch (AuthException ex)
        {
            ShowError(ex.Message);
        }
        catch (HttpRequestException)
        {
            ShowError("서버에 연결할 수 없습니다. 백엔드가 실행 중인지 확인하세요.");
        }
        catch (Exception ex)
        {
            ShowError($"오류: {ex.Message}");
        }
        finally
        {
            SetBusy(false);
        }
    }

    private void ShowError(string message)
    {
        errorLabel.Text = message;
        errorLabel.Visible = true;
    }

    private void SetBusy(bool busy)
    {
        _busy = busy;
        registerButton.Enabled = !busy;
        registerButton.Text = busy ? "가입 중..." : "가입하기";
        if (busy) errorLabel.Visible = false;
    }
    private void CancelButton_Click(object? sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }
}
