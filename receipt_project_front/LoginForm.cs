namespace receipt_project_front;

public partial class LoginForm : Form
{
    public LoginForm()
    {
        InitializeComponent();
    }

    private void LoginButton_Click(object? sender, EventArgs e)
    {
        // Login flow is owned by another component; this just unblocks the UI.
        DialogResult = DialogResult.OK;
        Close();
    }
}
