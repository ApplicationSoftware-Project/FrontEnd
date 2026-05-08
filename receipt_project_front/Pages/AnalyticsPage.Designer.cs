using receipt_project_front.UI;

namespace receipt_project_front.Pages;

partial class AnalyticsPage
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
    private FlowLayoutPanel subNav;
    private LinkLabel monthlyLink;
    private LinkLabel dailyLink;
    private Panel contentCard;
    private Label placeholderLabel;

    private void InitializeComponent()
    {
        rootLayout = new TableLayoutPanel();
        titleLabel = new Label();
        subNav = new FlowLayoutPanel();
        monthlyLink = new LinkLabel();
        dailyLink = new LinkLabel();
        contentCard = new Panel();
        placeholderLabel = new Label();

        SuspendLayout();

        // titleLabel
        titleLabel.AutoSize = false;
        titleLabel.Dock = DockStyle.Fill;
        titleLabel.Font = AppTheme.H1;
        titleLabel.ForeColor = AppTheme.TextPrimary;
        titleLabel.Text = "통계";
        titleLabel.TextAlign = ContentAlignment.BottomLeft;

        // monthlyLink
        monthlyLink.AutoSize = true;
        monthlyLink.Font = AppTheme.H3;
        monthlyLink.LinkBehavior = LinkBehavior.HoverUnderline;
        monthlyLink.LinkColor = AppTheme.TextPrimary;
        monthlyLink.ActiveLinkColor = AppTheme.Accent;
        monthlyLink.Margin = new Padding(0, 0, 24, 8);
        monthlyLink.Text = "•  월별";

        // dailyLink
        dailyLink.AutoSize = true;
        dailyLink.Font = AppTheme.H3;
        dailyLink.LinkBehavior = LinkBehavior.HoverUnderline;
        dailyLink.LinkColor = AppTheme.TextPrimary;
        dailyLink.ActiveLinkColor = AppTheme.Accent;
        dailyLink.Margin = new Padding(0, 0, 24, 0);
        dailyLink.Text = "•  일별";

        // subNav
        subNav.AutoSize = false;
        subNav.Controls.Add(monthlyLink);
        subNav.Controls.Add(dailyLink);
        subNav.Dock = DockStyle.Top;
        subNav.FlowDirection = FlowDirection.TopDown;
        subNav.Height = 80;
        subNav.Padding = new Padding(8, 8, 8, 8);

        // placeholderLabel
        placeholderLabel.AutoSize = false;
        placeholderLabel.Dock = DockStyle.Fill;
        placeholderLabel.Font = AppTheme.Body;
        placeholderLabel.ForeColor = AppTheme.TextMuted;
        placeholderLabel.Text = "통계 화면은 추후 구현 예정입니다.";
        placeholderLabel.TextAlign = ContentAlignment.MiddleCenter;

        // contentCard
        contentCard.BackColor = AppTheme.CardBg;
        contentCard.BorderStyle = BorderStyle.FixedSingle;
        contentCard.Controls.Add(placeholderLabel);
        contentCard.Controls.Add(subNav);
        contentCard.Dock = DockStyle.Fill;

        // rootLayout
        rootLayout.ColumnCount = 1;
        rootLayout.RowCount = 2;
        rootLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
        rootLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 60f));
        rootLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
        rootLayout.Controls.Add(titleLabel, 0, 0);
        rootLayout.Controls.Add(contentCard, 0, 1);
        rootLayout.Dock = DockStyle.Fill;
        rootLayout.Padding = new Padding(32);
        rootLayout.BackColor = AppTheme.ContentBg;

        // AnalyticsPage
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = AppTheme.ContentBg;
        Controls.Add(rootLayout);
        Name = "AnalyticsPage";
        Size = new Size(960, 680);
        ResumeLayout(false);
    }
}
