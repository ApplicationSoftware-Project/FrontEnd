using receipt_project_front.UI;

namespace receipt_project_front;

partial class LoginForm
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
            components.Dispose();
        base.Dispose(disposing);
    }

    private Panel sidebarPanel;
    private Panel sidebarHeader;
    private Label appTitleLabel;
    private Label appTaglineLabel;
    private Panel contentPanel;
    private Button loginButton;

    private void InitializeComponent()
    {
        sidebarPanel = new Panel();
        sidebarHeader = new Panel();
        appTitleLabel = new Label();
        appTaglineLabel = new Label();
        contentPanel = new Panel();
        loginButton = new Button();

        SuspendLayout();

        // sidebar header (matches MainForm shell)
        appTitleLabel.AutoSize = true;
        appTitleLabel.Font = new Font(AppTheme.FontFamily, 14f, FontStyle.Bold);
        appTitleLabel.ForeColor = Color.White;
        appTitleLabel.Location = new Point(20, 24);
        appTitleLabel.Text = "No More Receipts";

        appTaglineLabel.AutoSize = true;
        appTaglineLabel.Font = AppTheme.Caption;
        appTaglineLabel.ForeColor = AppTheme.SidebarTextDim;
        appTaglineLabel.Location = new Point(20, 50);
        appTaglineLabel.Text = "영수증, 더 이상 모으지 마세요.";

        sidebarHeader.BackColor = AppTheme.SidebarBgAlt;
        sidebarHeader.Controls.Add(appTitleLabel);
        sidebarHeader.Controls.Add(appTaglineLabel);
        sidebarHeader.Dock = DockStyle.Top;
        sidebarHeader.Height = 96;

        sidebarPanel.BackColor = AppTheme.SidebarBg;
        sidebarPanel.Controls.Add(sidebarHeader);
        sidebarPanel.Dock = DockStyle.Left;
        sidebarPanel.Width = 240;

        // loginButton
        loginButton.BackColor = AppTheme.Accent;
        loginButton.FlatAppearance.BorderSize = 0;
        loginButton.FlatStyle = FlatStyle.Flat;
        loginButton.Font = new Font(AppTheme.FontFamily, 11f, FontStyle.Bold);
        loginButton.ForeColor = Color.White;
        loginButton.Size = new Size(160, 48);
        loginButton.Text = "로그인";
        loginButton.UseVisualStyleBackColor = false;
        loginButton.Anchor = AnchorStyles.None;
        loginButton.Click += LoginButton_Click;

        // contentPanel
        contentPanel.BackColor = AppTheme.ContentBg;
        contentPanel.Controls.Add(loginButton);
        contentPanel.Dock = DockStyle.Fill;
        contentPanel.Resize += (_, _) =>
        {
            loginButton.Left = (contentPanel.ClientSize.Width - loginButton.Width) / 2;
            loginButton.Top = (contentPanel.ClientSize.Height - loginButton.Height) / 2;
        };

        // LoginForm
        AcceptButton = loginButton;
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = AppTheme.ContentBg;
        ClientSize = new Size(1024, 640);
        Controls.Add(contentPanel);
        Controls.Add(sidebarPanel);
        FormBorderStyle = FormBorderStyle.Sizable;
        MinimumSize = new Size(1024, 640);
        Name = "LoginForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "No More Receipts";
        ResumeLayout(false);
    }
}
