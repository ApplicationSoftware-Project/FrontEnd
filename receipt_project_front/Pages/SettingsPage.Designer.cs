using receipt_project_front.UI;

namespace receipt_project_front.Pages;

partial class SettingsPage
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
            components.Dispose();
        base.Dispose(disposing);
    }

    private TableLayoutPanel rootLayout;
    private Label titleLabel;
    private Panel profileCard;
    private Panel avatarPanel;
    private Label avatarInitialLabel;
    private Label profileNameLabel;
    private Label profileEmailLabel;
    private Label nameFieldLabel;
    private TextBox nameTextBox;
    private Label emailFieldLabel;
    private TextBox emailTextBox;
    private Label placeholderLabel;
    private Button signOutButton;

    private void InitializeComponent()
    {
        rootLayout = new TableLayoutPanel();
        titleLabel = new Label();
        profileCard = new Panel();
        avatarPanel = new Panel();
        avatarInitialLabel = new Label();
        profileNameLabel = new Label();
        profileEmailLabel = new Label();
        nameFieldLabel = new Label();
        nameTextBox = new TextBox();
        emailFieldLabel = new Label();
        emailTextBox = new TextBox();
        placeholderLabel = new Label();
        signOutButton = new Button();

        SuspendLayout();

        // titleLabel
        titleLabel.AutoSize = false;
        titleLabel.Dock = DockStyle.Fill;
        titleLabel.Font = AppTheme.H1;
        titleLabel.ForeColor = AppTheme.TextPrimary;
        titleLabel.Text = "설정";
        titleLabel.TextAlign = ContentAlignment.BottomLeft;

        // avatarInitialLabel
        avatarInitialLabel.AutoSize = false;
        avatarInitialLabel.Dock = DockStyle.Fill;
        avatarInitialLabel.Font = new Font(AppTheme.FontFamily, 28f, FontStyle.Bold);
        avatarInitialLabel.ForeColor = Color.White;
        avatarInitialLabel.Text = "U";
        avatarInitialLabel.TextAlign = ContentAlignment.MiddleCenter;

        // avatarPanel
        avatarPanel.BackColor = AppTheme.Accent;
        avatarPanel.Controls.Add(avatarInitialLabel);
        avatarPanel.Location = new Point(28, 28);
        avatarPanel.Size = new Size(80, 80);

        // profileNameLabel
        profileNameLabel.AutoSize = true;
        profileNameLabel.Font = AppTheme.H2;
        profileNameLabel.ForeColor = AppTheme.TextPrimary;
        profileNameLabel.Location = new Point(128, 38);
        profileNameLabel.Text = "사용자 이름";

        // profileEmailLabel
        profileEmailLabel.AutoSize = true;
        profileEmailLabel.Font = AppTheme.Body;
        profileEmailLabel.ForeColor = AppTheme.TextSecondary;
        profileEmailLabel.Location = new Point(128, 70);
        profileEmailLabel.Text = "user@example.com";

        // nameFieldLabel
        nameFieldLabel.AutoSize = true;
        nameFieldLabel.Font = AppTheme.BodyBold;
        nameFieldLabel.ForeColor = AppTheme.TextPrimary;
        nameFieldLabel.Location = new Point(28, 144);
        nameFieldLabel.Text = "이름";

        // nameTextBox
        nameTextBox.BorderStyle = BorderStyle.FixedSingle;
        nameTextBox.Enabled = false;
        nameTextBox.Font = new Font(AppTheme.FontFamily, 11f);
        nameTextBox.Location = new Point(28, 168);
        nameTextBox.Size = new Size(420, 28);
        nameTextBox.Text = "사용자 이름";

        // emailFieldLabel
        emailFieldLabel.AutoSize = true;
        emailFieldLabel.Font = AppTheme.BodyBold;
        emailFieldLabel.ForeColor = AppTheme.TextPrimary;
        emailFieldLabel.Location = new Point(28, 216);
        emailFieldLabel.Text = "이메일";

        // emailTextBox
        emailTextBox.BorderStyle = BorderStyle.FixedSingle;
        emailTextBox.Enabled = false;
        emailTextBox.Font = new Font(AppTheme.FontFamily, 11f);
        emailTextBox.Location = new Point(28, 240);
        emailTextBox.Size = new Size(420, 28);
        emailTextBox.Text = "user@example.com";

        // placeholderLabel
        placeholderLabel.AutoSize = true;
        placeholderLabel.Font = AppTheme.Caption;
        placeholderLabel.ForeColor = AppTheme.TextMuted;
        placeholderLabel.Location = new Point(28, 284);
        placeholderLabel.Text = "프로필 편집은 추후 구현 예정입니다.";

        // signOutButton
        signOutButton.BackColor = Color.White;
        signOutButton.FlatAppearance.BorderColor = AppTheme.Danger;
        signOutButton.FlatAppearance.BorderSize = 1;
        signOutButton.FlatStyle = FlatStyle.Flat;
        signOutButton.Font = AppTheme.BodyBold;
        signOutButton.ForeColor = AppTheme.Danger;
        signOutButton.Location = new Point(28, 332);
        signOutButton.Size = new Size(180, 44);
        signOutButton.Text = "로그아웃";
        signOutButton.UseVisualStyleBackColor = false;
        signOutButton.Click += SignOutButton_Click;

        // profileCard
        profileCard.BackColor = AppTheme.CardBg;
        profileCard.BorderStyle = BorderStyle.FixedSingle;
        profileCard.Controls.Add(avatarPanel);
        profileCard.Controls.Add(profileNameLabel);
        profileCard.Controls.Add(profileEmailLabel);
        profileCard.Controls.Add(nameFieldLabel);
        profileCard.Controls.Add(nameTextBox);
        profileCard.Controls.Add(emailFieldLabel);
        profileCard.Controls.Add(emailTextBox);
        profileCard.Controls.Add(placeholderLabel);
        profileCard.Controls.Add(signOutButton);
        profileCard.Dock = DockStyle.Fill;

        // rootLayout
        rootLayout.ColumnCount = 1;
        rootLayout.RowCount = 2;
        rootLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
        rootLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 60f));
        rootLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
        rootLayout.Controls.Add(titleLabel, 0, 0);
        rootLayout.Controls.Add(profileCard, 0, 1);
        rootLayout.Dock = DockStyle.Fill;
        rootLayout.Padding = new Padding(32);
        rootLayout.BackColor = AppTheme.ContentBg;

        // SettingsPage
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = AppTheme.ContentBg;
        Controls.Add(rootLayout);
        Name = "SettingsPage";
        Size = new Size(960, 680);
        ResumeLayout(false);
    }
}
