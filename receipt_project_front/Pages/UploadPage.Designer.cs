using receipt_project_front.UI;

namespace receipt_project_front.Pages;

partial class UploadPage
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
            components.Dispose();
        base.Dispose(disposing);
    }

    private Panel dropZone;
    private Label dropArrowLabel;
    private Label dropHintLabel;

    private void InitializeComponent()
    {
        dropZone = new Panel();
        dropArrowLabel = new Label();
        dropHintLabel = new Label();

        SuspendLayout();

        // dropArrowLabel
        dropArrowLabel.AutoSize = false;
        dropArrowLabel.Dock = DockStyle.Fill;
        dropArrowLabel.Font = new Font(AppTheme.FontFamily, 80f, FontStyle.Regular);
        dropArrowLabel.ForeColor = AppTheme.Accent;
        dropArrowLabel.Text = "↑";
        dropArrowLabel.TextAlign = ContentAlignment.MiddleCenter;
        dropArrowLabel.Cursor = Cursors.Hand;

        // dropHintLabel
        dropHintLabel.AutoSize = false;
        dropHintLabel.Dock = DockStyle.Bottom;
        dropHintLabel.Font = AppTheme.Body;
        dropHintLabel.ForeColor = AppTheme.TextSecondary;
        dropHintLabel.Height = 36;
        dropHintLabel.Text = "클릭하여 영수증 업로드";
        dropHintLabel.TextAlign = ContentAlignment.MiddleCenter;
        dropHintLabel.Cursor = Cursors.Hand;

        // dropZone
        dropZone.BackColor = AppTheme.CardBg;
        dropZone.BorderStyle = BorderStyle.FixedSingle;
        dropZone.Controls.Add(dropArrowLabel);
        dropZone.Controls.Add(dropHintLabel);
        dropZone.Cursor = Cursors.Hand;
        dropZone.Size = new Size(280, 280);
        dropZone.Anchor = AnchorStyles.None;

        // UploadPage
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = AppTheme.ContentBg;
        Controls.Add(dropZone);
        Name = "UploadPage";
        Size = new Size(960, 680);
        Resize += (_, _) => CenterDropZone();
        Load += (_, _) => CenterDropZone();
        ResumeLayout(false);
    }

    private void CenterDropZone()
    {
        dropZone.Left = Math.Max(0, (ClientSize.Width - dropZone.Width) / 2);
        dropZone.Top = Math.Max(0, (ClientSize.Height - dropZone.Height) / 2);
    }
}
