using System.Globalization;
using receipt_project_front.Models;
using receipt_project_front.UI;

namespace receipt_project_front.Pages;

public partial class ReceiptBrowserPage : UserControl, IRefreshablePage
{
    private readonly List<ReceiptSummary> _receipts = new();
    private int _index;

    public ReceiptBrowserPage()
    {
        InitializeComponent();
        UpdateView();
    }

    public void OnNavigatedTo()
    {
        // TODO: GET /api/receipts and populate _receipts; reset _index to 0.
        UpdateView();
    }

    private void PrevButton_Click(object? sender, EventArgs e)
    {
        if (_receipts.Count == 0) return;
        _index = (_index - 1 + _receipts.Count) % _receipts.Count;
        UpdateView();
    }

    private void NextButton_Click(object? sender, EventArgs e)
    {
        if (_receipts.Count == 0) return;
        _index = (_index + 1) % _receipts.Count;
        UpdateView();
    }

    private void UpdateView()
    {
        if (_receipts.Count == 0)
        {
            categoryStoreLabel.Text = "표시할 영수증이 없습니다";
            dateLabel.Text = string.Empty;
            amountLabel.Text = "-";
            prevButton.Enabled = false;
            nextButton.Enabled = false;
            photoBox.Image?.Dispose();
            photoBox.Image = null;
            return;
        }

        var r = _receipts[_index];
        categoryStoreLabel.Text = $"{r.Category ?? r.AiSuggestedCategory ?? "분류 없음"}  |  {r.StoreName}";
        dateLabel.Text = (r.PurchasedAt ?? r.CreatedAt).LocalDateTime.ToString("yyyy-MM-dd HH:mm");
        amountLabel.Text = r.Amount.HasValue
            ? "합계 " + r.Amount.Value.ToString("N0", CultureInfo.InvariantCulture)
            : "-";

        prevButton.Enabled = _receipts.Count > 1;
        nextButton.Enabled = _receipts.Count > 1;

        // TODO: GET /api/receipts/{id}/image and load into photoBox.
    }
}
