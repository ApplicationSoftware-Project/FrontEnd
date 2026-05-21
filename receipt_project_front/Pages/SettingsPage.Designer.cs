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

    // ── 레이아웃 ──────────────────────────────────────
    private TableLayoutPanel rootLayout;
    private Label titleLabel;
    private Panel profileCard;

    // ── 프로필 헤더 ───────────────────────────────────
    private Panel avatarPanel;
    private Label avatarInitialLabel;
    private Label profileNameLabel;
    private Label profileEmailLabel;

    // ── 프로필 수정 ───────────────────────────────────
    private Label nameFieldLabel;
    private TextBox nameTextBox;
    private Label emailFieldLabel;
    private TextBox emailTextBox;
    private Button saveProfileButton;
    private Label profileStatusLabel;

    // ── 비밀번호 변경 ─────────────────────────────────
    private Label passwordSectionLabel;
    private Label currentPasswordLabel;
    private TextBox currentPasswordBox;
    private Label newPasswordLabel;
    private TextBox newPasswordBox;
    private Label confirmPasswordLabel;
    private TextBox confirmPasswordBox;
    private Button changePasswordButton;
    private Label passwordStatusLabel;

    // ── 알림 설정 ─────────────────────────────────────
    private Label notificationSectionLabel;
    private CheckBox emailNotificationCheckBox;
    private CheckBox pushNotificationCheckBox;
    private Button saveNotificationsButton;
    private Label notificationStatusLabel;

    // ── 개발자 (임시) ─────────────────────────────────
    private Label devSectionHeaderLabel;
    private Label devSectionWarningLabel;
    private Label devTokenLabel;
    private TextBox devTokenTextBox;
    private Button saveTokenButton;
    private Label tokenStatusLabel;

    // ── 로그아웃 ──────────────────────────────────────
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
        saveProfileButton = new Button();
        profileStatusLabel = new Label();

        passwordSectionLabel = new Label();
        currentPasswordLabel = new Label();
        currentPasswordBox = new TextBox();
        newPasswordLabel = new Label();
        newPasswordBox = new TextBox();
        confirmPasswordLabel = new Label();
        confirmPasswordBox = new TextBox();
        changePasswordButton = new Button();
        passwordStatusLabel = new Label();

        notificationSectionLabel = new Label();
        emailNotificationCheckBox = new CheckBox();
        pushNotificationCheckBox = new CheckBox();
        saveNotificationsButton = new Button();
        notificationStatusLabel = new Label();

        devSectionHeaderLabel = new Label();
        devSectionWarningLabel = new Label();
        devTokenLabel = new Label();
        devTokenTextBox = new TextBox();
        saveTokenButton = new Button();
        tokenStatusLabel = new Label();

        signOutButton = new Button();

        SuspendLayout();

        // ── titleLabel ────────────────────────────────
        titleLabel.AutoSize = false;
        titleLabel.Dock = DockStyle.Fill;
        titleLabel.Font = AppTheme.H1;
        titleLabel.ForeColor = AppTheme.TextPrimary;
        titleLabel.Text = "설정";
        titleLabel.TextAlign = ContentAlignment.BottomLeft;

        // ── 프로필 헤더 ───────────────────────────────
        avatarInitialLabel.AutoSize = false;
        avatarInitialLabel.Dock = DockStyle.Fill;
        avatarInitialLabel.Font = new Font(AppTheme.FontFamily, 28f, FontStyle.Bold);
        avatarInitialLabel.ForeColor = Color.White;
        avatarInitialLabel.Text = "U";
        avatarInitialLabel.TextAlign = ContentAlignment.MiddleCenter;

        avatarPanel.BackColor = AppTheme.Accent;
        avatarPanel.Controls.Add(avatarInitialLabel);
        avatarPanel.Location = new Point(28, 28);
        avatarPanel.Size = new Size(80, 80);

        profileNameLabel.AutoSize = true;
        profileNameLabel.Font = AppTheme.H2;
        profileNameLabel.ForeColor = AppTheme.TextPrimary;
        profileNameLabel.Location = new Point(128, 38);
        profileNameLabel.Text = "사용자 이름";

        profileEmailLabel.AutoSize = true;
        profileEmailLabel.Font = AppTheme.Body;
        profileEmailLabel.ForeColor = AppTheme.TextSecondary;
        profileEmailLabel.Location = new Point(128, 70);
        profileEmailLabel.Text = "user@example.com";

        // ── 프로필 수정 ───────────────────────────────
        nameFieldLabel.AutoSize = true;
        nameFieldLabel.Font = AppTheme.BodyBold;
        nameFieldLabel.ForeColor = AppTheme.TextPrimary;
        nameFieldLabel.Location = new Point(28, 140);
        nameFieldLabel.Text = "이름";

        nameTextBox.BorderStyle = BorderStyle.FixedSingle;
        nameTextBox.Font = new Font(AppTheme.FontFamily, 11f);
        nameTextBox.Location = new Point(28, 164);
        nameTextBox.Size = new Size(420, 28);

        emailFieldLabel.AutoSize = true;
        emailFieldLabel.Font = AppTheme.BodyBold;
        emailFieldLabel.ForeColor = AppTheme.TextPrimary;
        emailFieldLabel.Location = new Point(28, 208);
        emailFieldLabel.Text = "이메일 (변경 불가)";

        emailTextBox.BorderStyle = BorderStyle.FixedSingle;
        emailTextBox.Enabled = false;
        emailTextBox.Font = new Font(AppTheme.FontFamily, 11f);
        emailTextBox.Location = new Point(28, 232);
        emailTextBox.Size = new Size(420, 28);
        emailTextBox.BackColor = Color.FromArgb(241, 245, 249);

        saveProfileButton.BackColor = AppTheme.Accent;
        saveProfileButton.FlatAppearance.BorderSize = 0;
        saveProfileButton.FlatStyle = FlatStyle.Flat;
        saveProfileButton.Font = AppTheme.BodyBold;
        saveProfileButton.ForeColor = Color.White;
        saveProfileButton.Location = new Point(28, 280);
        saveProfileButton.Size = new Size(160, 36);
        saveProfileButton.Text = "프로필 저장";
        saveProfileButton.UseVisualStyleBackColor = false;
        saveProfileButton.Click += SaveProfileButton_Click;

        profileStatusLabel.AutoSize = true;
        profileStatusLabel.Font = AppTheme.Body;
        profileStatusLabel.ForeColor = AppTheme.TextMuted;
        profileStatusLabel.Location = new Point(200, 290);
        profileStatusLabel.Text = string.Empty;

        // ── 구분선 ────────────────────────────────────
        // ── 비밀번호 변경 ─────────────────────────────
        passwordSectionLabel.AutoSize = true;
        passwordSectionLabel.Font = AppTheme.H3;
        passwordSectionLabel.ForeColor = AppTheme.TextPrimary;
        passwordSectionLabel.Location = new Point(28, 348);
        passwordSectionLabel.Text = "비밀번호 변경";

        currentPasswordLabel.AutoSize = true;
        currentPasswordLabel.Font = AppTheme.BodyBold;
        currentPasswordLabel.ForeColor = AppTheme.TextPrimary;
        currentPasswordLabel.Location = new Point(28, 380);
        currentPasswordLabel.Text = "현재 비밀번호";

        currentPasswordBox.BorderStyle = BorderStyle.FixedSingle;
        currentPasswordBox.Font = new Font(AppTheme.FontFamily, 11f);
        currentPasswordBox.Location = new Point(28, 404);
        currentPasswordBox.PasswordChar = '●';
        currentPasswordBox.Size = new Size(420, 28);

        newPasswordLabel.AutoSize = true;
        newPasswordLabel.Font = AppTheme.BodyBold;
        newPasswordLabel.ForeColor = AppTheme.TextPrimary;
        newPasswordLabel.Location = new Point(28, 448);
        newPasswordLabel.Text = "새 비밀번호 (6자 이상)";

        newPasswordBox.BorderStyle = BorderStyle.FixedSingle;
        newPasswordBox.Font = new Font(AppTheme.FontFamily, 11f);
        newPasswordBox.Location = new Point(28, 472);
        newPasswordBox.PasswordChar = '●';
        newPasswordBox.Size = new Size(420, 28);

        confirmPasswordLabel.AutoSize = true;
        confirmPasswordLabel.Font = AppTheme.BodyBold;
        confirmPasswordLabel.ForeColor = AppTheme.TextPrimary;
        confirmPasswordLabel.Location = new Point(28, 516);
        confirmPasswordLabel.Text = "새 비밀번호 확인";

        confirmPasswordBox.BorderStyle = BorderStyle.FixedSingle;
        confirmPasswordBox.Font = new Font(AppTheme.FontFamily, 11f);
        confirmPasswordBox.Location = new Point(28, 540);
        confirmPasswordBox.PasswordChar = '●';
        confirmPasswordBox.Size = new Size(420, 28);

        changePasswordButton.BackColor = AppTheme.Accent;
        changePasswordButton.FlatAppearance.BorderSize = 0;
        changePasswordButton.FlatStyle = FlatStyle.Flat;
        changePasswordButton.Font = AppTheme.BodyBold;
        changePasswordButton.ForeColor = Color.White;
        changePasswordButton.Location = new Point(28, 588);
        changePasswordButton.Size = new Size(160, 36);
        changePasswordButton.Text = "비밀번호 변경";
        changePasswordButton.UseVisualStyleBackColor = false;
        changePasswordButton.Click += ChangePasswordButton_Click;

        passwordStatusLabel.AutoSize = true;
        passwordStatusLabel.Font = AppTheme.Body;
        passwordStatusLabel.ForeColor = AppTheme.TextMuted;
        passwordStatusLabel.Location = new Point(200, 598);
        passwordStatusLabel.Text = string.Empty;

        // ── 알림 설정 ─────────────────────────────────
        notificationSectionLabel.AutoSize = true;
        notificationSectionLabel.Font = AppTheme.H3;
        notificationSectionLabel.ForeColor = AppTheme.TextPrimary;
        notificationSectionLabel.Location = new Point(28, 656);
        notificationSectionLabel.Text = "알림 설정";

        emailNotificationCheckBox.AutoSize = true;
        emailNotificationCheckBox.Font = AppTheme.Body;
        emailNotificationCheckBox.ForeColor = AppTheme.TextPrimary;
        emailNotificationCheckBox.Location = new Point(28, 690);
        emailNotificationCheckBox.Text = "이메일 알림 수신";

        pushNotificationCheckBox.AutoSize = true;
        pushNotificationCheckBox.Font = AppTheme.Body;
        pushNotificationCheckBox.ForeColor = AppTheme.TextPrimary;
        pushNotificationCheckBox.Location = new Point(28, 720);
        pushNotificationCheckBox.Text = "푸시 알림 수신";

        saveNotificationsButton.BackColor = AppTheme.Accent;
        saveNotificationsButton.FlatAppearance.BorderSize = 0;
        saveNotificationsButton.FlatStyle = FlatStyle.Flat;
        saveNotificationsButton.Font = AppTheme.BodyBold;
        saveNotificationsButton.ForeColor = Color.White;
        saveNotificationsButton.Location = new Point(28, 756);
        saveNotificationsButton.Size = new Size(160, 36);
        saveNotificationsButton.Text = "알림 저장";
        saveNotificationsButton.UseVisualStyleBackColor = false;
        saveNotificationsButton.Click += SaveNotificationsButton_Click;

        notificationStatusLabel.AutoSize = true;
        notificationStatusLabel.Font = AppTheme.Body;
        notificationStatusLabel.ForeColor = AppTheme.TextMuted;
        notificationStatusLabel.Location = new Point(200, 766);
        notificationStatusLabel.Text = string.Empty;

        // ── 개발자 (임시) ─────────────────────────────
        devSectionHeaderLabel.AutoSize = true;
        devSectionHeaderLabel.Font = AppTheme.H3;
        devSectionHeaderLabel.ForeColor = AppTheme.TextPrimary;
        devSectionHeaderLabel.Location = new Point(28, 824);
        devSectionHeaderLabel.Text = "개발자 (임시)";

        devSectionWarningLabel.AutoSize = true;
        devSectionWarningLabel.Font = AppTheme.Caption;
        devSectionWarningLabel.ForeColor = AppTheme.Danger;
        devSectionWarningLabel.Location = new Point(28, 850);
        devSectionWarningLabel.Text = "로그인 UI 구현 전까지 임시 사용. 프로덕션 빌드 전에 제거됩니다.";

        devTokenLabel.AutoSize = true;
        devTokenLabel.Font = AppTheme.BodyBold;
        devTokenLabel.ForeColor = AppTheme.TextPrimary;
        devTokenLabel.Location = new Point(28, 880);
        devTokenLabel.Text = "Dev Access Token (JWT)";

        devTokenTextBox.BorderStyle = BorderStyle.FixedSingle;
        devTokenTextBox.Font = new Font("Consolas", 9.5f);
        devTokenTextBox.Location = new Point(28, 904);
        devTokenTextBox.Multiline = true;
        devTokenTextBox.ScrollBars = ScrollBars.Vertical;
        devTokenTextBox.Size = new Size(680, 80);
        devTokenTextBox.WordWrap = true;

        saveTokenButton.BackColor = AppTheme.Accent;
        saveTokenButton.FlatAppearance.BorderSize = 0;
        saveTokenButton.FlatStyle = FlatStyle.Flat;
        saveTokenButton.Font = AppTheme.BodyBold;
        saveTokenButton.ForeColor = Color.White;
        saveTokenButton.Location = new Point(28, 996);
        saveTokenButton.Size = new Size(120, 36);
        saveTokenButton.Text = "저장";
        saveTokenButton.UseVisualStyleBackColor = false;
        saveTokenButton.Click += SaveTokenButton_Click;

        tokenStatusLabel.AutoSize = true;
        tokenStatusLabel.Font = AppTheme.Body;
        tokenStatusLabel.ForeColor = AppTheme.TextMuted;
        tokenStatusLabel.Location = new Point(160, 1006);
        tokenStatusLabel.Text = "토큰 없음";

        // ── 로그아웃 ──────────────────────────────────
        signOutButton.BackColor = Color.White;
        signOutButton.FlatAppearance.BorderColor = AppTheme.Danger;
        signOutButton.FlatAppearance.BorderSize = 1;
        signOutButton.FlatStyle = FlatStyle.Flat;
        signOutButton.Font = AppTheme.BodyBold;
        signOutButton.ForeColor = AppTheme.Danger;
        signOutButton.Location = new Point(28, 1060);
        signOutButton.Size = new Size(180, 44);
        signOutButton.Text = "로그아웃";
        signOutButton.UseVisualStyleBackColor = false;
        signOutButton.Click += SignOutButton_Click;

        // ── profileCard ───────────────────────────────
        profileCard.AutoScroll = true;
        profileCard.BackColor = AppTheme.CardBg;
        profileCard.BorderStyle = BorderStyle.FixedSingle;
        profileCard.Controls.Add(avatarPanel);
        profileCard.Controls.Add(profileNameLabel);
        profileCard.Controls.Add(profileEmailLabel);
        profileCard.Controls.Add(nameFieldLabel);
        profileCard.Controls.Add(nameTextBox);
        profileCard.Controls.Add(emailFieldLabel);
        profileCard.Controls.Add(emailTextBox);
        profileCard.Controls.Add(saveProfileButton);
        profileCard.Controls.Add(profileStatusLabel);
        profileCard.Controls.Add(passwordSectionLabel);
        profileCard.Controls.Add(currentPasswordLabel);
        profileCard.Controls.Add(currentPasswordBox);
        profileCard.Controls.Add(newPasswordLabel);
        profileCard.Controls.Add(newPasswordBox);
        profileCard.Controls.Add(confirmPasswordLabel);
        profileCard.Controls.Add(confirmPasswordBox);
        profileCard.Controls.Add(changePasswordButton);
        profileCard.Controls.Add(passwordStatusLabel);
        profileCard.Controls.Add(notificationSectionLabel);
        profileCard.Controls.Add(emailNotificationCheckBox);
        profileCard.Controls.Add(pushNotificationCheckBox);
        profileCard.Controls.Add(saveNotificationsButton);
        profileCard.Controls.Add(notificationStatusLabel);
        profileCard.Controls.Add(devSectionHeaderLabel);
        profileCard.Controls.Add(devSectionWarningLabel);
        profileCard.Controls.Add(devTokenLabel);
        profileCard.Controls.Add(devTokenTextBox);
        profileCard.Controls.Add(saveTokenButton);
        profileCard.Controls.Add(tokenStatusLabel);
        profileCard.Controls.Add(signOutButton);
        profileCard.Dock = DockStyle.Fill;

        // ── rootLayout ────────────────────────────────
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

        // ── SettingsPage ──────────────────────────────
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = AppTheme.ContentBg;
        Controls.Add(rootLayout);
        Name = "SettingsPage";
        Size = new Size(960, 680);
        ResumeLayout(false);
    }
}