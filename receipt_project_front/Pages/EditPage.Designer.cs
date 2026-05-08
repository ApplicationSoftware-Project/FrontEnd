using receipt_project_front.UI;

namespace receipt_project_front.Pages;

partial class EditPage
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
    private Label storeLabel;
    private TextBox storeTextBox;
    private Label amountLabel;
    private TextBox amountTextBox;
    private Label purchasedAtLabel;
    private DateTimePicker purchasedAtPicker;
    private Label categoryLabel;
    private TextBox categoryTextBox;
    private Label aiSuggestionLabel;
    private Button submitButton;

    private void InitializeComponent()
    {
        rootLayout = new TableLayoutPanel();
        titleLabel = new Label();
        splitLayout = new TableLayoutPanel();
        photoCard = new Panel();
        photoBox = new PictureBox();
        infoCard = new Panel();
        storeLabel = new Label();
        storeTextBox = new TextBox();
        amountLabel = new Label();
        amountTextBox = new TextBox();
        purchasedAtLabel = new Label();
        purchasedAtPicker = new DateTimePicker();
        categoryLabel = new Label();
        categoryTextBox = new TextBox();
        aiSuggestionLabel = new Label();
        submitButton = new Button();

        SuspendLayout();

        // titleLabel
        titleLabel.AutoSize = false;
        titleLabel.Dock = DockStyle.Fill;
        titleLabel.Font = AppTheme.H1;
        titleLabel.ForeColor = AppTheme.TextPrimary;
        titleLabel.Text = "편집";
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

        // storeLabel
        storeLabel.AutoSize = true;
        storeLabel.Font = AppTheme.BodyBold;
        storeLabel.ForeColor = AppTheme.TextPrimary;
        storeLabel.Location = new Point(28, 28);
        storeLabel.Text = "상호명";

        // storeTextBox
        storeTextBox.BorderStyle = BorderStyle.FixedSingle;
        storeTextBox.Font = new Font(AppTheme.FontFamily, 11f);
        storeTextBox.Location = new Point(28, 52);
        storeTextBox.Size = new Size(320, 28);

        // amountLabel
        amountLabel.AutoSize = true;
        amountLabel.Font = AppTheme.BodyBold;
        amountLabel.ForeColor = AppTheme.TextPrimary;
        amountLabel.Location = new Point(28, 100);
        amountLabel.Text = "금액";

        // amountTextBox
        amountTextBox.BorderStyle = BorderStyle.FixedSingle;
        amountTextBox.Font = new Font(AppTheme.FontFamily, 11f);
        amountTextBox.Location = new Point(28, 124);
        amountTextBox.Size = new Size(320, 28);

        // purchasedAtLabel
        purchasedAtLabel.AutoSize = true;
        purchasedAtLabel.Font = AppTheme.BodyBold;
        purchasedAtLabel.ForeColor = AppTheme.TextPrimary;
        purchasedAtLabel.Location = new Point(28, 172);
        purchasedAtLabel.Text = "구매일";

        // purchasedAtPicker
        purchasedAtPicker.Font = new Font(AppTheme.FontFamily, 11f);
        purchasedAtPicker.Format = DateTimePickerFormat.Custom;
        purchasedAtPicker.CustomFormat = "yyyy-MM-dd";
        purchasedAtPicker.Location = new Point(28, 196);
        purchasedAtPicker.Size = new Size(320, 28);

        // categoryLabel
        categoryLabel.AutoSize = true;
        categoryLabel.Font = AppTheme.BodyBold;
        categoryLabel.ForeColor = AppTheme.TextPrimary;
        categoryLabel.Location = new Point(28, 244);
        categoryLabel.Text = "카테고리";

        // categoryTextBox
        categoryTextBox.BorderStyle = BorderStyle.FixedSingle;
        categoryTextBox.Font = new Font(AppTheme.FontFamily, 11f);
        categoryTextBox.Location = new Point(28, 268);
        categoryTextBox.Size = new Size(320, 28);

        // aiSuggestionLabel
        aiSuggestionLabel.AutoSize = true;
        aiSuggestionLabel.Font = AppTheme.Caption;
        aiSuggestionLabel.ForeColor = AppTheme.TextSecondary;
        aiSuggestionLabel.Location = new Point(28, 304);
        aiSuggestionLabel.Text = "AI 추천: 없음";

        // submitButton
        submitButton.BackColor = AppTheme.Accent;
        submitButton.FlatAppearance.BorderSize = 0;
        submitButton.FlatStyle = FlatStyle.Flat;
        submitButton.Font = new Font(AppTheme.FontFamily, 11f, FontStyle.Bold);
        submitButton.ForeColor = Color.White;
        submitButton.Location = new Point(28, 360);
        submitButton.Size = new Size(320, 48);
        submitButton.Text = "저장";
        submitButton.UseVisualStyleBackColor = false;
        submitButton.Click += SubmitButton_Click;

        // infoCard
        infoCard.BackColor = AppTheme.CardBg;
        infoCard.BorderStyle = BorderStyle.FixedSingle;
        infoCard.Controls.Add(storeLabel);
        infoCard.Controls.Add(storeTextBox);
        infoCard.Controls.Add(amountLabel);
        infoCard.Controls.Add(amountTextBox);
        infoCard.Controls.Add(purchasedAtLabel);
        infoCard.Controls.Add(purchasedAtPicker);
        infoCard.Controls.Add(categoryLabel);
        infoCard.Controls.Add(categoryTextBox);
        infoCard.Controls.Add(aiSuggestionLabel);
        infoCard.Controls.Add(submitButton);
        infoCard.Dock = DockStyle.Fill;
        infoCard.Margin = new Padding(8, 0, 0, 0);

        // splitLayout
        splitLayout.ColumnCount = 2;
        splitLayout.RowCount = 1;
        splitLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 55f));
        splitLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45f));
        splitLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
        splitLayout.Controls.Add(photoCard, 0, 0);
        splitLayout.Controls.Add(infoCard, 1, 0);
        splitLayout.Dock = DockStyle.Fill;
        splitLayout.Margin = new Padding(0);

        // rootLayout
        rootLayout.ColumnCount = 1;
        rootLayout.RowCount = 2;
        rootLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
        rootLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 60f));
        rootLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
        rootLayout.Controls.Add(titleLabel, 0, 0);
        rootLayout.Controls.Add(splitLayout, 0, 1);
        rootLayout.Dock = DockStyle.Fill;
        rootLayout.Padding = new Padding(32);
        rootLayout.BackColor = AppTheme.ContentBg;

        // EditPage
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = AppTheme.ContentBg;
        Controls.Add(rootLayout);
        Name = "EditPage";
        Size = new Size(960, 680);
        ResumeLayout(false);
    }
}
