using receipt_project_front.UI;

namespace receipt_project_front;

partial class MainForm
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

    private Button btnUpload;
    private Button btnOverview;
    private Button btnBrowser;
    private Button btnAnalytics;
    private Button btnSettings;

    private void InitializeComponent()
    {
        sidebarPanel = new Panel();
        sidebarHeader = new Panel();
        appTitleLabel = new Label();
        appTaglineLabel = new Label();
        contentPanel = new Panel();

        btnUpload = MakeNavButton("영수증 업로드", NavItem.Upload);
        btnOverview = MakeNavButton("Overview", NavItem.Overview);
        btnBrowser = MakeNavButton("영수증 조회", NavItem.Browser);
        btnAnalytics = MakeNavButton("통계", NavItem.Analytics);
        btnSettings = MakeNavButton("설정", NavItem.Settings);

        _navButtons[NavItem.Upload] = btnUpload;
        _navButtons[NavItem.Overview] = btnOverview;
        _navButtons[NavItem.Browser] = btnBrowser;
        _navButtons[NavItem.Analytics] = btnAnalytics;
        _navButtons[NavItem.Settings] = btnSettings;

        SuspendLayout();

        // appTitleLabel
        appTitleLabel.AutoSize = true;
        appTitleLabel.Font = new Font(AppTheme.FontFamily, 14f, FontStyle.Bold);
        appTitleLabel.ForeColor = Color.White;
        appTitleLabel.Location = new Point(20, 24);
        appTitleLabel.Text = "No More Receipts";

        // appTaglineLabel
        appTaglineLabel.AutoSize = true;
        appTaglineLabel.Font = AppTheme.Caption;
        appTaglineLabel.ForeColor = AppTheme.SidebarTextDim;
        appTaglineLabel.Location = new Point(20, 50);
        appTaglineLabel.Text = "영수증, 더 이상 모으지 마세요.";

        // sidebarHeader
        sidebarHeader.BackColor = AppTheme.SidebarBgAlt;
        sidebarHeader.Controls.Add(appTitleLabel);
        sidebarHeader.Controls.Add(appTaglineLabel);
        sidebarHeader.Dock = DockStyle.Top;
        sidebarHeader.Height = 96;

        btnUpload.Top = 112;
        btnOverview.Top = 164;
        btnBrowser.Top = 216;
        btnAnalytics.Top = 268;
        btnSettings.Top = 320;

        // sidebarPanel
        sidebarPanel.BackColor = AppTheme.SidebarBg;
        sidebarPanel.Controls.Add(btnSettings);
        sidebarPanel.Controls.Add(btnAnalytics);
        sidebarPanel.Controls.Add(btnBrowser);
        sidebarPanel.Controls.Add(btnOverview);
        sidebarPanel.Controls.Add(btnUpload);
        sidebarPanel.Controls.Add(sidebarHeader);
        sidebarPanel.Dock = DockStyle.Left;
        sidebarPanel.Width = 240;

        // contentPanel
        contentPanel.BackColor = AppTheme.ContentBg;
        contentPanel.Dock = DockStyle.Fill;
        contentPanel.Padding = new Padding(0);

        // MainForm
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = AppTheme.ContentBg;
        ClientSize = new Size(1200, 780);
        Controls.Add(contentPanel);
        Controls.Add(sidebarPanel);
        Font = AppTheme.Body;
        MinimumSize = new Size(1024, 640);
        Name = "MainForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "No More Receipts";
        ResumeLayout(false);
    }

    private Button MakeNavButton(string text, NavItem item)
    {
        var btn = new Button
        {
            Text = "  " + text,
            TextAlign = ContentAlignment.MiddleLeft,
            FlatStyle = FlatStyle.Flat,
            BackColor = AppTheme.SidebarBg,
            ForeColor = AppTheme.SidebarText,
            Font = new Font(AppTheme.FontFamily, 10.5f, FontStyle.Regular),
            Size = new Size(220, 44),
            Left = 10,
            Cursor = Cursors.Hand,
            UseVisualStyleBackColor = false
        };
        btn.FlatAppearance.BorderSize = 0;
        btn.FlatAppearance.MouseOverBackColor = AppTheme.SidebarHover;
        btn.Click += (s, e) => Navigate(item);
        return btn;
    }
}
