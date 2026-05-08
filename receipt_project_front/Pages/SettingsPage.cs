namespace receipt_project_front.Pages;

public partial class SettingsPage : UserControl
{
    public event EventHandler? SignOutRequested;

    public SettingsPage()
    {
        InitializeComponent();
    }

    private void SignOutButton_Click(object? sender, EventArgs e)
    {
        SignOutRequested?.Invoke(this, EventArgs.Empty);
    }
}
