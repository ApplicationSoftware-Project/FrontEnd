using receipt_project_front.UI;

namespace receipt_project_front.Pages;

partial class OverviewPage
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
    private Panel monthlyCard;
    private Label monthlyTitleLabel;
    private Label monthlyAmountLabel;
    private Label monthlyCaptionLabel;
    private Panel receiptsCard;
    private Label receiptsTitleLabel;
    private Panel receiptsListPanel;

    private void InitializeComponent()
    {
        rootLayout = new TableLayoutPanel();
        titleLabel = new Label();
        monthlyCard = new Panel();
        monthlyTitleLabel = new Label();
        monthlyAmountLabel = new Label();
        monthlyCaptionLabel = new Label();
        receiptsCard = new Panel();
        receiptsTitleLabel = new Label();
        receiptsListPanel = new Panel();

        SuspendLayout();

        // titleLabel
        titleLabel.AutoSize = false;
        titleLabel.Dock = DockStyle.Fill;
        titleLabel.Font = AppTheme.H1;
        titleLabel.ForeColor = AppTheme.TextPrimary;
        titleLabel.Text = "개요";
        titleLabel.TextAlign = ContentAlignment.BottomLeft;
        titleLabel.Margin = new Padding(0, 0, 0, 12);

        // monthlyTitleLabel
        monthlyTitleLabel.AutoSize = true;
        monthlyTitleLabel.Font = AppTheme.H3;
        monthlyTitleLabel.ForeColor = AppTheme.TextSecondary;
        monthlyTitleLabel.Location = new Point(28, 24);
        monthlyTitleLabel.Text = "이번 달 지출";

        // monthlyAmountLabel
        monthlyAmountLabel.AutoSize = true;
        monthlyAmountLabel.Font = AppTheme.Display;
        monthlyAmountLabel.ForeColor = AppTheme.TextPrimary;
        monthlyAmountLabel.Location = new Point(24, 56);
        monthlyAmountLabel.Text = "₩0";

        // monthlyCaptionLabel
        monthlyCaptionLabel.AutoSize = true;
        monthlyCaptionLabel.Font = AppTheme.Caption;
        monthlyCaptionLabel.ForeColor = AppTheme.TextMuted;
        monthlyCaptionLabel.Location = new Point(28, 168);
        // 실제 텍스트(예: "2026년 6월")는 OverviewPage에서 현재 월 기준으로 설정한다.
        monthlyCaptionLabel.Text = string.Empty;

        // monthlyCard
        monthlyCard.BackColor = AppTheme.CardBg;
        monthlyCard.BorderStyle = BorderStyle.FixedSingle;
        monthlyCard.Controls.Add(monthlyTitleLabel);
        monthlyCard.Controls.Add(monthlyAmountLabel);
        monthlyCard.Controls.Add(monthlyCaptionLabel);
        monthlyCard.Dock = DockStyle.Fill;
        monthlyCard.Margin = new Padding(0, 0, 0, 16);

        // receiptsTitleLabel
        receiptsTitleLabel.BackColor = Color.Transparent;
        receiptsTitleLabel.Dock = DockStyle.Top;
        receiptsTitleLabel.Font = AppTheme.H3;
        receiptsTitleLabel.ForeColor = AppTheme.TextPrimary;
        receiptsTitleLabel.Height = 48;
        receiptsTitleLabel.Padding = new Padding(20, 16, 20, 0);
        receiptsTitleLabel.Text = "최근 영수증";
        receiptsTitleLabel.TextAlign = ContentAlignment.MiddleLeft;

        // receiptsListPanel
        receiptsListPanel.BackColor = AppTheme.CardBg;
        receiptsListPanel.Dock = DockStyle.Fill;
        receiptsListPanel.AutoScroll = true;
        receiptsListPanel.Padding = new Padding(0, 0, 0, 0);

        // receiptsCard
        receiptsCard.BackColor = AppTheme.CardBg;
        receiptsCard.BorderStyle = BorderStyle.FixedSingle;
        receiptsCard.Controls.Add(receiptsListPanel);
        receiptsCard.Controls.Add(receiptsTitleLabel);
        receiptsCard.Dock = DockStyle.Fill;
        receiptsCard.Margin = new Padding(0);

        // rootLayout
        rootLayout.ColumnCount = 1;
        rootLayout.RowCount = 3;
        rootLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
        rootLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 60f));
        rootLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 220f));
        rootLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
        rootLayout.Controls.Add(titleLabel, 0, 0);
        rootLayout.Controls.Add(monthlyCard, 0, 1);
        rootLayout.Controls.Add(receiptsCard, 0, 2);
        rootLayout.Dock = DockStyle.Fill;
        rootLayout.Padding = new Padding(32);
        rootLayout.BackColor = AppTheme.ContentBg;

        // OverviewPage
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = AppTheme.ContentBg;
        Controls.Add(rootLayout);
        Name = "OverviewPage";
        Size = new Size(960, 680);
        ResumeLayout(false);
    }
}
