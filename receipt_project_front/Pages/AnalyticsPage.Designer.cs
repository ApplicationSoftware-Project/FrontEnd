using LiveChartsCore.SkiaSharpView.WinForms;
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
    private LinkLabel categoryLink;
    private Panel categoryHeader;
    private Label monthSelectorLabel;
    private ComboBox monthSelector;
    private Panel contentCard;
    private Label statusLabel;
    private CartesianChart cartesianChart;

    private void InitializeComponent()
    {
        rootLayout = new TableLayoutPanel();
        titleLabel = new Label();
        subNav = new FlowLayoutPanel();
        monthlyLink = new LinkLabel();
        dailyLink = new LinkLabel();
        categoryLink = new LinkLabel();
        categoryHeader = new Panel();
        monthSelectorLabel = new Label();
        monthSelector = new ComboBox();
        contentCard = new Panel();
        statusLabel = new Label();
        cartesianChart = new CartesianChart();

        SuspendLayout();

        // titleLabel
        titleLabel.AutoSize = false;
        titleLabel.Dock = DockStyle.Fill;
        titleLabel.Font = AppTheme.H1;
        titleLabel.ForeColor = AppTheme.TextPrimary;
        titleLabel.Text = "통계";
        titleLabel.TextAlign = ContentAlignment.BottomLeft;

        // monthlyLink — active by default
        monthlyLink.AutoSize = true;
        monthlyLink.Font = AppTheme.H3;
        monthlyLink.LinkBehavior = LinkBehavior.HoverUnderline;
        monthlyLink.LinkColor = AppTheme.Accent;
        monthlyLink.ActiveLinkColor = AppTheme.Accent;
        monthlyLink.Margin = new Padding(0, 0, 24, 8);
        monthlyLink.Text = "•  월별";

        // dailyLink — inactive by default
        dailyLink.AutoSize = true;
        dailyLink.Font = AppTheme.H3;
        dailyLink.LinkBehavior = LinkBehavior.HoverUnderline;
        dailyLink.LinkColor = AppTheme.TextSecondary;
        dailyLink.ActiveLinkColor = AppTheme.Accent;
        dailyLink.Margin = new Padding(0, 0, 24, 8);
        dailyLink.Text = "•  일별";

        // categoryLink — inactive by default
        categoryLink.AutoSize = true;
        categoryLink.Font = AppTheme.H3;
        categoryLink.LinkBehavior = LinkBehavior.HoverUnderline;
        categoryLink.LinkColor = AppTheme.TextSecondary;
        categoryLink.ActiveLinkColor = AppTheme.Accent;
        categoryLink.Margin = new Padding(0, 0, 24, 0);
        categoryLink.Text = "•  카테고리별";

        // subNav — taller to accommodate 3 links
        subNav.AutoSize = false;
        subNav.Controls.Add(monthlyLink);
        subNav.Controls.Add(dailyLink);
        subNav.Controls.Add(categoryLink);
        subNav.Dock = DockStyle.Top;
        subNav.FlowDirection = FlowDirection.TopDown;
        subNav.Height = 108;
        subNav.Padding = new Padding(8, 8, 8, 8);

        // monthSelectorLabel
        monthSelectorLabel.AutoSize = true;
        monthSelectorLabel.Font = AppTheme.Body;
        monthSelectorLabel.ForeColor = AppTheme.TextPrimary;
        monthSelectorLabel.Text = "월 선택:";
        monthSelectorLabel.TextAlign = ContentAlignment.MiddleLeft;
        monthSelectorLabel.Margin = new Padding(0, 4, 6, 0);

        // monthSelector
        monthSelector.DropDownStyle = ComboBoxStyle.DropDownList;
        monthSelector.Width = 140;
        monthSelector.Font = AppTheme.Body;
        monthSelector.Margin = new Padding(0, 0, 0, 0);

        // categoryHeader — right-aligned month picker, hidden unless Category view is active
        // Uses an inner FlowLayoutPanel (RightToLeft) so the label+combobox hug the right edge.
        var headerFlow = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.RightToLeft,
            WrapContents = false,
            AutoSize = false,
            Padding = new Padding(0, 8, 8, 0)
        };
        // In RightToLeft flow: first added = rightmost
        headerFlow.Controls.Add(monthSelector);
        headerFlow.Controls.Add(monthSelectorLabel);

        categoryHeader.BackColor = AppTheme.CardBg;
        categoryHeader.Dock = DockStyle.Top;
        categoryHeader.Height = 44;
        categoryHeader.Visible = false;
        categoryHeader.Controls.Add(headerFlow);

        // statusLabel
        statusLabel.AutoSize = false;
        statusLabel.Dock = DockStyle.Fill;
        statusLabel.Font = AppTheme.Body;
        statusLabel.ForeColor = AppTheme.TextMuted;
        statusLabel.Text = "불러오는 중...";
        statusLabel.TextAlign = ContentAlignment.MiddleCenter;
        statusLabel.Visible = true;

        // cartesianChart
        cartesianChart.Dock = DockStyle.Fill;
        cartesianChart.Visible = false;

        // contentCard — Controls.Add order controls docking precedence:
        //   later-added Dock=Top controls occupy outer (higher) positions.
        //   subNav added last → very top; categoryHeader added before it → just below subNav.
        //   Both Dock=Fill controls (statusLabel on top of cartesianChart) fill remaining space.
        contentCard.BackColor = AppTheme.CardBg;
        contentCard.BorderStyle = BorderStyle.FixedSingle;
        contentCard.Controls.Add(cartesianChart);
        contentCard.Controls.Add(statusLabel);
        contentCard.Controls.Add(categoryHeader);
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
