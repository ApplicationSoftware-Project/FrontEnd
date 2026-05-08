using receipt_project_front.UI;

namespace receipt_project_front;

partial class RegisterForm
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
    private Label titleLabel;
    private TextBox emailBox;
    private TextBox displayNameBox;
    private TextBox passwordBox;
    private TextBox confirmBox;
    private Label errorLabel;
    private Button registerButton;
    private Button cancelButton;

    private void InitializeComponent()
    {
        sidebarPanel = new Panel();
        sidebarHeader = new Panel();
        appTitleLabel = new Label();
        appTaglineLabel = new Label();
        contentPanel = new Panel();
        titleLabel = new Label();
        emailBox = new TextBox();
        displayNameBox = new TextBox();
        passwordBox = new TextBox();
        confirmBox = new TextBox();
        errorLabel = new Label();
        registerButton = new Button();
        cancelButton = new Button();
        sidebarPanel.SuspendLayout();
        sidebarHeader.SuspendLayout();
        contentPanel.SuspendLayout();
        SuspendLayout();
        // 
        // sidebarPanel
        // 
        sidebarPanel.BackColor = Color.FromArgb(30, 41, 59);
        sidebarPanel.Controls.Add(sidebarHeader);
        sidebarPanel.Dock = DockStyle.Left;
        sidebarPanel.Location = new Point(0, 0);
        sidebarPanel.Margin = new Padding(4, 5, 4, 5);
        sidebarPanel.Name = "sidebarPanel";
        sidebarPanel.Size = new Size(343, 967);
        sidebarPanel.TabIndex = 1;
        // 
        // sidebarHeader
        // 
        sidebarHeader.BackColor = Color.FromArgb(15, 23, 42);
        sidebarHeader.Controls.Add(appTitleLabel);
        sidebarHeader.Controls.Add(appTaglineLabel);
        sidebarHeader.Dock = DockStyle.Top;
        sidebarHeader.Location = new Point(0, 0);
        sidebarHeader.Margin = new Padding(4, 5, 4, 5);
        sidebarHeader.Name = "sidebarHeader";
        sidebarHeader.Size = new Size(343, 160);
        sidebarHeader.TabIndex = 0;
        // 
        // appTitleLabel
        // 
        appTitleLabel.AutoSize = true;
        appTitleLabel.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
        appTitleLabel.ForeColor = Color.White;
        appTitleLabel.Location = new Point(29, 40);
        appTitleLabel.Margin = new Padding(4, 0, 4, 0);
        appTitleLabel.Name = "appTitleLabel";
        appTitleLabel.Size = new Size(250, 38);
        appTitleLabel.TabIndex = 0;
        appTitleLabel.Text = "No More Receipts";
        // 
        // appTaglineLabel
        // 
        appTaglineLabel.AutoSize = true;
        appTaglineLabel.Font = new Font("Segoe UI", 9F);
        appTaglineLabel.ForeColor = Color.FromArgb(148, 163, 184);
        appTaglineLabel.Location = new Point(29, 83);
        appTaglineLabel.Margin = new Padding(4, 0, 4, 0);
        appTaglineLabel.Name = "appTaglineLabel";
        appTaglineLabel.Size = new Size(256, 25);
        appTaglineLabel.TabIndex = 1;
        appTaglineLabel.Text = "영수증, 더 이상 모으지 마세요.";
        // 
        // contentPanel
        // 
        contentPanel.BackColor = Color.FromArgb(248, 250, 252);
        contentPanel.Controls.Add(titleLabel);
        contentPanel.Controls.Add(emailBox);
        contentPanel.Controls.Add(displayNameBox);
        contentPanel.Controls.Add(passwordBox);
        contentPanel.Controls.Add(confirmBox);
        contentPanel.Controls.Add(errorLabel);
        contentPanel.Controls.Add(registerButton);
        contentPanel.Controls.Add(cancelButton);
        contentPanel.Dock = DockStyle.Fill;
        contentPanel.Location = new Point(343, 0);
        contentPanel.Margin = new Padding(4, 5, 4, 5);
        contentPanel.Name = "contentPanel";
        contentPanel.Size = new Size(771, 967);
        contentPanel.TabIndex = 0;
        // 
        // titleLabel
        // 
        titleLabel.Anchor = AnchorStyles.None;
        titleLabel.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
        titleLabel.ForeColor = Color.FromArgb(15, 23, 42);
        titleLabel.Location = new Point(239, 163);
        titleLabel.Margin = new Padding(4, 0, 4, 0);
        titleLabel.Name = "titleLabel";
        titleLabel.Size = new Size(429, 60);
        titleLabel.TabIndex = 0;
        titleLabel.Text = "회원가입";
        titleLabel.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // emailBox
        // 
        emailBox.Anchor = AnchorStyles.None;
        emailBox.BorderStyle = BorderStyle.FixedSingle;
        emailBox.Font = new Font("Segoe UI", 12F);
        emailBox.Location = new Point(240, 259);
        emailBox.Margin = new Padding(4, 5, 4, 5);
        emailBox.Name = "emailBox";
        emailBox.PlaceholderText = "이메일";
        emailBox.Size = new Size(428, 39);
        emailBox.TabIndex = 1;
        // 
        // displayNameBox
        // 
        displayNameBox.Anchor = AnchorStyles.None;
        displayNameBox.BorderStyle = BorderStyle.FixedSingle;
        displayNameBox.Font = new Font("Segoe UI", 12F);
        displayNameBox.Location = new Point(241, 322);
        displayNameBox.Margin = new Padding(4, 5, 4, 5);
        displayNameBox.Name = "displayNameBox";
        displayNameBox.PlaceholderText = "닉네임 (표시 이름)";
        displayNameBox.Size = new Size(428, 39);
        displayNameBox.TabIndex = 2;
        // 
        // passwordBox
        // 
        passwordBox.Anchor = AnchorStyles.None;
        passwordBox.BorderStyle = BorderStyle.FixedSingle;
        passwordBox.Font = new Font("Segoe UI", 12F);
        passwordBox.Location = new Point(240, 386);
        passwordBox.Margin = new Padding(4, 5, 4, 5);
        passwordBox.Name = "passwordBox";
        passwordBox.PasswordChar = '●';
        passwordBox.PlaceholderText = "비밀번호 (6자 이상)";
        passwordBox.Size = new Size(428, 39);
        passwordBox.TabIndex = 3;
        // 
        // confirmBox
        // 
        confirmBox.Anchor = AnchorStyles.None;
        confirmBox.BorderStyle = BorderStyle.FixedSingle;
        confirmBox.Font = new Font("Segoe UI", 12F);
        confirmBox.Location = new Point(243, 447);
        confirmBox.Margin = new Padding(4, 5, 4, 5);
        confirmBox.Name = "confirmBox";
        confirmBox.PasswordChar = '●';
        confirmBox.PlaceholderText = "비밀번호 확인";
        confirmBox.Size = new Size(428, 39);
        confirmBox.TabIndex = 4;
        // 
        // errorLabel
        // 
        errorLabel.Anchor = AnchorStyles.None;
        errorLabel.Font = new Font("Segoe UI", 9F);
        errorLabel.ForeColor = Color.FromArgb(239, 68, 68);
        errorLabel.Location = new Point(240, 523);
        errorLabel.Margin = new Padding(4, 0, 4, 0);
        errorLabel.Name = "errorLabel";
        errorLabel.Size = new Size(429, 33);
        errorLabel.TabIndex = 5;
        errorLabel.TextAlign = ContentAlignment.MiddleCenter;
        errorLabel.Visible = false;
        // 
        // registerButton
        // 
        registerButton.Anchor = AnchorStyles.None;
        registerButton.BackColor = Color.FromArgb(59, 130, 246);
        registerButton.FlatAppearance.BorderSize = 0;
        registerButton.FlatStyle = FlatStyle.Flat;
        registerButton.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
        registerButton.ForeColor = Color.White;
        registerButton.Location = new Point(240, 619);
        registerButton.Margin = new Padding(4, 5, 4, 5);
        registerButton.Name = "registerButton";
        registerButton.Size = new Size(429, 73);
        registerButton.TabIndex = 6;
        registerButton.Text = "가입하기";
        registerButton.UseVisualStyleBackColor = false;
        // 
        // cancelButton
        // 
        cancelButton.Anchor = AnchorStyles.None;
        cancelButton.BackColor = Color.Transparent;
        cancelButton.FlatAppearance.BorderSize = 0;
        cancelButton.FlatStyle = FlatStyle.Flat;
        cancelButton.Font = new Font("Segoe UI", 9F);
        cancelButton.ForeColor = Color.FromArgb(100, 116, 139);
        cancelButton.Location = new Point(240, 720);
        cancelButton.Margin = new Padding(4, 5, 4, 5);
        cancelButton.Name = "cancelButton";
        cancelButton.Size = new Size(429, 40);
        cancelButton.TabIndex = 7;
        cancelButton.Text = "취소";
        cancelButton.UseVisualStyleBackColor = false;
        cancelButton.Click += CancelButton_Click;
        // 
        // RegisterForm
        // 
        AcceptButton = registerButton;
        AutoScaleDimensions = new SizeF(10F, 25F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(248, 250, 252);
        CancelButton = cancelButton;
        ClientSize = new Size(1114, 967);
        Controls.Add(contentPanel);
        Controls.Add(sidebarPanel);
        Margin = new Padding(4, 5, 4, 5);
        MinimumSize = new Size(1105, 929);
        Name = "RegisterForm";
        StartPosition = FormStartPosition.CenterParent;
        Text = "No More Receipts - 회원가입";
        sidebarPanel.ResumeLayout(false);
        sidebarHeader.ResumeLayout(false);
        sidebarHeader.PerformLayout();
        contentPanel.ResumeLayout(false);
        contentPanel.PerformLayout();
        ResumeLayout(false);
    }


}
