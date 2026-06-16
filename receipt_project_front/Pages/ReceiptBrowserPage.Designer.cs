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
    private Panel topBar;
    private Label monthCaptionLabel;
    private ComboBox monthSelector;
    private TableLayoutPanel mainSplit;
    private Panel listCard;
    private Panel listPanel;
    private TableLayoutPanel detailLayout;
    private Panel photoCard;
    private PictureBox photoBox;
    private Panel infoCard;
    private FlowLayoutPanel topRowFlow;
    private Label categoryLabel;
    private Label separatorLabel;
    private Label storeNameLabel;
    private Label dateLabel;
    private Label amountLabel;
    private Panel actionPanel;
    private Button deleteButton;

    private void InitializeComponent()
    {
        rootLayout = new TableLayoutPanel();
        titleLabel = new Label();
        topBar = new Panel();
        monthCaptionLabel = new Label();
        monthSelector = new ComboBox();
        mainSplit = new TableLayoutPanel();
        listCard = new Panel();
        listPanel = new Panel();
        detailLayout = new TableLayoutPanel();
        photoCard = new Panel();
        photoBox = new PictureBox();
        infoCard = new Panel();
        topRowFlow = new FlowLayoutPanel();
        categoryLabel = new Label();
        separatorLabel = new Label();
        storeNameLabel = new Label();
        dateLabel = new Label();
        amountLabel = new Label();
        actionPanel = new Panel();
        deleteButton = new Button();

        SuspendLayout();

        // titleLabel
        titleLabel.AutoSize = false;
        titleLabel.Dock = DockStyle.Fill;
        titleLabel.Font = AppTheme.H1;
        titleLabel.ForeColor = AppTheme.TextPrimary;
        titleLabel.Text = "영수증 조회";
        titleLabel.TextAlign = ContentAlignment.BottomLeft;

        // monthCaptionLabel
        monthCaptionLabel.AutoSize = true;
        monthCaptionLabel.Font = AppTheme.Body;
        monthCaptionLabel.ForeColor = AppTheme.TextSecondary;
        monthCaptionLabel.Location = new Point(2, 14);
        monthCaptionLabel.Text = "월 선택";

        // monthSelector
        monthSelector.DropDownStyle = ComboBoxStyle.DropDownList;
        monthSelector.FlatStyle = FlatStyle.Flat;
        monthSelector.Font = AppTheme.Body;
        monthSelector.Location = new Point(72, 10);
        monthSelector.Size = new Size(180, 28);
        monthSelector.SelectedIndexChanged += MonthSelector_SelectedIndexChanged;

        // topBar
        topBar.Controls.Add(monthCaptionLabel);
        topBar.Controls.Add(monthSelector);
        topBar.Dock = DockStyle.Fill;

        // listPanel (스크롤되는 영수증 목록 컨테이너)
        listPanel.AutoScroll = true;
        listPanel.BackColor = AppTheme.CardBg;
        listPanel.Dock = DockStyle.Fill;

        // listCard
        listCard.BackColor = AppTheme.CardBg;
        listCard.BorderStyle = BorderStyle.FixedSingle;
        listCard.Controls.Add(listPanel);
        listCard.Dock = DockStyle.Fill;
        listCard.Margin = new Padding(0, 0, 8, 0);

        // photoBox
        photoBox.BackColor = AppTheme.ContentBg;
        photoBox.Dock = DockStyle.Fill;
        photoBox.SizeMode = PictureBoxSizeMode.Zoom;

        // photoCard
        photoCard.BackColor = AppTheme.CardBg;
        photoCard.BorderStyle = BorderStyle.FixedSingle;
        photoCard.Controls.Add(photoBox);
        photoCard.Dock = DockStyle.Fill;
        photoCard.Margin = new Padding(8, 0, 0, 8);
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

        // topRowFlow
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

        // infoCard (Dock=Top stack: amount, date, topRow — Dock=Top stacks bottom-first)
        infoCard.BackColor = AppTheme.CardBg;
        infoCard.BorderStyle = BorderStyle.FixedSingle;
        infoCard.Controls.Add(amountLabel);
        infoCard.Controls.Add(dateLabel);
        infoCard.Controls.Add(topRowFlow);
        infoCard.Dock = DockStyle.Fill;
        infoCard.Margin = new Padding(8, 0, 0, 8);

        // deleteButton (현재 영수증 삭제)
        deleteButton.BackColor = AppTheme.Danger;
        deleteButton.FlatAppearance.BorderSize = 0;
        deleteButton.FlatStyle = FlatStyle.Flat;
        deleteButton.Font = new Font(AppTheme.FontFamily, 11f, FontStyle.Bold);
        deleteButton.ForeColor = Color.White;
        deleteButton.Dock = DockStyle.Right;
        deleteButton.Width = 120;
        deleteButton.Text = "🗑  삭제";
        deleteButton.UseVisualStyleBackColor = false;
        deleteButton.Click += DeleteButton_Click;

        // actionPanel
        actionPanel.Controls.Add(deleteButton);
        actionPanel.Dock = DockStyle.Fill;
        actionPanel.Margin = new Padding(8, 0, 0, 0);
        actionPanel.Padding = new Padding(0, 4, 0, 4);

        // detailLayout (오른쪽 상세: 사진 / 정보 / 액션)
        detailLayout.ColumnCount = 1;
        detailLayout.RowCount = 3;
        detailLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
        detailLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 58f));
        detailLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 42f));
        detailLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 60f));
        detailLayout.Controls.Add(photoCard, 0, 0);
        detailLayout.Controls.Add(infoCard, 0, 1);
        detailLayout.Controls.Add(actionPanel, 0, 2);
        detailLayout.Dock = DockStyle.Fill;
        detailLayout.Margin = new Padding(0);

        // mainSplit (왼쪽 목록 | 오른쪽 상세)
        mainSplit.ColumnCount = 2;
        mainSplit.RowCount = 1;
        mainSplit.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 38f));
        mainSplit.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 62f));
        mainSplit.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
        mainSplit.Controls.Add(listCard, 0, 0);
        mainSplit.Controls.Add(detailLayout, 1, 0);
        mainSplit.Dock = DockStyle.Fill;
        mainSplit.Margin = new Padding(0);

        // rootLayout
        rootLayout.ColumnCount = 1;
        rootLayout.RowCount = 3;
        rootLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
        rootLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 60f));
        rootLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 48f));
        rootLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
        rootLayout.Controls.Add(titleLabel, 0, 0);
        rootLayout.Controls.Add(topBar, 0, 1);
        rootLayout.Controls.Add(mainSplit, 0, 2);
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
