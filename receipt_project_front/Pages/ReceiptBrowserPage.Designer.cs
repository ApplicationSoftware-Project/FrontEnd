using receipt_project_front.UI;

namespace receipt_project_front.Pages;

partial class ReceiptBrowserPage
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
    private TableLayoutPanel splitLayout;
    private Panel photoCard;
    private PictureBox photoBox;
    private Panel infoCard;
    private FlowLayoutPanel topRowFlow;
    private Label categoryLabel;
    private Label separatorLabel;
    private Label storeNameLabel;
    private Label dateLabel;
    private Label amountLabel;
    private Panel navPanel;
    private Button prevButton;
    private Button nextButton;
    private Button deleteButton;

    private void InitializeComponent()
    {
        rootLayout = new TableLayoutPanel();
        titleLabel = new Label();
        splitLayout = new TableLayoutPanel();
        photoCard = new Panel();
        photoBox = new PictureBox();
        infoCard = new Panel();
        topRowFlow = new FlowLayoutPanel();
        categoryLabel = new Label();
        separatorLabel = new Label();
        storeNameLabel = new Label();
        dateLabel = new Label();
        amountLabel = new Label();
        navPanel = new Panel();
        prevButton = new Button();
        nextButton = new Button();
        deleteButton = new Button();

        SuspendLayout();

        // titleLabel
        titleLabel.AutoSize = false;
        titleLabel.Dock = DockStyle.Fill;
        titleLabel.Font = AppTheme.H1;
        titleLabel.ForeColor = AppTheme.TextPrimary;
        titleLabel.Text = "영수증 조회";
        titleLabel.TextAlign = ContentAlignment.BottomLeft;

        // photoBox
        photoBox.BackColor = AppTheme.ContentBg;
        photoBox.Dock = DockStyle.Fill;
        photoBox.SizeMode = PictureBoxSizeMode.Zoom;

        // photoCard
        photoCard.BackColor = AppTheme.CardBg;
        photoCard.BorderStyle = BorderStyle.FixedSingle;
        photoCard.Controls.Add(photoBox);
        photoCard.Dock = DockStyle.Fill;
        photoCard.Margin = new Padding(0, 0, 8, 0);
        photoCard.Padding = new Padding(8);

        // categoryLabel (editable)
        categoryLabel.AutoSize = true;
        categoryLabel.Font = AppTheme.H2;
        categoryLabel.ForeColor = AppTheme.TextPrimary;
        categoryLabel.Margin = new Padding(0, 0, 0, 0);
        categoryLabel.Text = "카테고리";

        // separatorLabel (static, not editable)
        separatorLabel.AutoSize = true;
        separatorLabel.Font = AppTheme.H2;
        separatorLabel.ForeColor = AppTheme.TextSecondary;
        separatorLabel.Margin = new Padding(8, 0, 8, 0);
        separatorLabel.Text = "|";

        // storeNameLabel (editable)
        storeNameLabel.AutoSize = true;
        storeNameLabel.Font = AppTheme.H2;
        storeNameLabel.ForeColor = AppTheme.TextPrimary;
        storeNameLabel.Margin = new Padding(0, 0, 0, 0);
        storeNameLabel.Text = "상호명";

        // topRowFlow — height covers the H2 font (~25px) plus top padding plus
        // bottom breathing room, so dateLabel below doesn't overlap.
        topRowFlow.AutoSize = false;
        topRowFlow.Controls.Add(categoryLabel);
        topRowFlow.Controls.Add(separatorLabel);
        topRowFlow.Controls.Add(storeNameLabel);
        topRowFlow.Dock = DockStyle.Top;
        topRowFlow.FlowDirection = FlowDirection.LeftToRight;
        topRowFlow.Height = 60;
        topRowFlow.Padding = new Padding(24, 20, 24, 8);
        topRowFlow.WrapContents = false;

        // dateLabel (editable)
        dateLabel.AutoSize = false;
        dateLabel.Dock = DockStyle.Top;
        dateLabel.Font = AppTheme.Body;
        dateLabel.ForeColor = AppTheme.TextSecondary;
        dateLabel.Height = 28;
        dateLabel.Padding = new Padding(24, 0, 24, 0);
        dateLabel.Text = "yyyy-MM-dd HH:mm";
        dateLabel.TextAlign = ContentAlignment.TopLeft;

        // amountLabel (editable)
        amountLabel.AutoSize = false;
        amountLabel.Dock = DockStyle.Top;
        amountLabel.Font = AppTheme.H2;
        amountLabel.ForeColor = AppTheme.TextPrimary;
        amountLabel.Height = 64;
        amountLabel.Padding = new Padding(24, 24, 24, 0);
        amountLabel.Text = "합계 -";
        amountLabel.TextAlign = ContentAlignment.MiddleLeft;

        // infoCard (Dock=Top stack: amount, date, topRow — order is reversed because Dock=Top stacks bottom-first)
        infoCard.BackColor = AppTheme.CardBg;
        infoCard.BorderStyle = BorderStyle.FixedSingle;
        infoCard.Controls.Add(amountLabel);
        infoCard.Controls.Add(dateLabel);
        infoCard.Controls.Add(topRowFlow);
        infoCard.Dock = DockStyle.Fill;
        infoCard.Margin = new Padding(8, 0, 0, 0);

        // splitLayout
        splitLayout.ColumnCount = 2;
        splitLayout.RowCount = 1;
        splitLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
        splitLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
        splitLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
        splitLayout.Controls.Add(photoCard, 0, 0);
        splitLayout.Controls.Add(infoCard, 1, 0);
        splitLayout.Dock = DockStyle.Fill;
        splitLayout.Margin = new Padding(0);

        // prevButton (◀)
        prevButton.BackColor = AppTheme.CardBg;
        prevButton.FlatAppearance.BorderColor = AppTheme.CardBorder;
        prevButton.FlatAppearance.BorderSize = 1;
        prevButton.FlatStyle = FlatStyle.Flat;
        prevButton.Font = new Font(AppTheme.FontFamily, 18f, FontStyle.Bold);
        prevButton.ForeColor = AppTheme.TextPrimary;
        prevButton.Location = new Point(0, 0);
        prevButton.Size = new Size(56, 56);
        prevButton.Text = "‹";
        prevButton.UseVisualStyleBackColor = false;
        prevButton.Anchor = AnchorStyles.Top | AnchorStyles.Left;
        prevButton.Click += PrevButton_Click;

        // nextButton (▶)
        nextButton.BackColor = AppTheme.CardBg;
        nextButton.FlatAppearance.BorderColor = AppTheme.CardBorder;
        nextButton.FlatAppearance.BorderSize = 1;
        nextButton.FlatStyle = FlatStyle.Flat;
        nextButton.Font = new Font(AppTheme.FontFamily, 18f, FontStyle.Bold);
        nextButton.ForeColor = AppTheme.TextPrimary;
        nextButton.Size = new Size(56, 56);
        nextButton.Text = "›";
        nextButton.UseVisualStyleBackColor = false;
        nextButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        nextButton.Click += NextButton_Click;

        // deleteButton (현재 영수증 삭제) — navPanel 중앙
        deleteButton.BackColor = AppTheme.Danger;
        deleteButton.FlatAppearance.BorderSize = 0;
        deleteButton.FlatStyle = FlatStyle.Flat;
        deleteButton.Font = new Font(AppTheme.FontFamily, 11f, FontStyle.Bold);
        deleteButton.ForeColor = Color.White;
        deleteButton.Size = new Size(120, 56);
        deleteButton.Text = "🗑  삭제";
        deleteButton.UseVisualStyleBackColor = false;
        deleteButton.Anchor = AnchorStyles.Top;
        deleteButton.Click += DeleteButton_Click;

        // navPanel
        navPanel.Controls.Add(prevButton);
        navPanel.Controls.Add(nextButton);
        navPanel.Controls.Add(deleteButton);
        navPanel.Dock = DockStyle.Bottom;
        navPanel.Height = 72;
        navPanel.Padding = new Padding(0, 8, 0, 0);
        navPanel.Resize += (_, _) =>
        {
            nextButton.Left = navPanel.ClientSize.Width - nextButton.Width;
            deleteButton.Left = (navPanel.ClientSize.Width - deleteButton.Width) / 2;
        };

        // rootLayout
        rootLayout.ColumnCount = 1;
        rootLayout.RowCount = 3;
        rootLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
        rootLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 60f));
        rootLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
        rootLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 80f));
        rootLayout.Controls.Add(titleLabel, 0, 0);
        rootLayout.Controls.Add(splitLayout, 0, 1);
        rootLayout.Controls.Add(navPanel, 0, 2);
        rootLayout.Dock = DockStyle.Fill;
        rootLayout.Padding = new Padding(32);
        rootLayout.BackColor = AppTheme.ContentBg;

        // ReceiptBrowserPage
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = AppTheme.ContentBg;
        Controls.Add(rootLayout);
        Name = "ReceiptBrowserPage";
        Size = new Size(960, 680);
        ResumeLayout(false);
    }
}
