using System.Globalization;
using receipt_project_front.Models;
using receipt_project_front.Services;
using receipt_project_front.UI;

namespace receipt_project_front.Pages;

public partial class OverviewPage : UserControl, IRefreshablePage
{
    public OverviewPage()
    {
        InitializeComponent();
        SetMonthlyExpense(0m);
        SetRecentReceipts(Array.Empty<ReceiptSummary>());
    }

    public async void OnNavigatedTo()
    {
        SetMonthlyExpense(0m);
        try
        {
            var trend = await ReceiptApi.GetMonthlyTrendAsync();
            var now = DateTime.Now;
            var thisMonth = trend.FirstOrDefault(t => t.Year == now.Year && t.Month == now.Month);
            SetMonthlyExpense(thisMonth is not null ? (decimal)thisMonth.TotalAmount : 0m);
        }
        catch { }
    }

    public void SetMonthlyExpense(decimal amount)
    {
        monthlyAmountLabel.Text = "₩" + amount.ToString("N0", CultureInfo.InvariantCulture);
    }

    public void SetRecentReceipts(IReadOnlyList<ReceiptSummary> receipts)
    {
        receiptsListPanel.SuspendLayout();
        receiptsListPanel.Controls.Clear();

        if (receipts.Count == 0)
        {
            var empty = new Label
            {
                Text = "최근 등록한 영수증이 없습니다",
                Font = AppTheme.Body,
                ForeColor = AppTheme.TextSecondary,
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill
            };
            receiptsListPanel.Controls.Add(empty);
        }
        else
        {
            foreach (var r in receipts.Take(5))
                receiptsListPanel.Controls.Add(BuildRow(r));
        }

        receiptsListPanel.ResumeLayout();
    }

    private static Panel BuildRow(ReceiptSummary r)
    {
        var row = new Panel { Height = 56, Dock = DockStyle.Top, Padding = new Padding(16, 12, 16, 12) };

        var store = new Label
        {
            Text = string.IsNullOrWhiteSpace(r.StoreName) ? "(이름 없음)" : r.StoreName,
            Font = AppTheme.BodyBold,
            ForeColor = AppTheme.TextPrimary,
            AutoSize = true,
            Location = new Point(16, 10)
        };

        var date = new Label
        {
            Text = (r.PurchasedAt ?? r.CreatedAt).LocalDateTime.ToString("yyyy-MM-dd"),
            Font = AppTheme.Caption,
            ForeColor = AppTheme.TextSecondary,
            AutoSize = true,
            Location = new Point(16, 30)
        };

        var amount = new Label
        {
            Text = r.Amount.HasValue ? "₩" + r.Amount.Value.ToString("N0", CultureInfo.InvariantCulture) : "-",
            Font = AppTheme.BodyBold,
            ForeColor = AppTheme.TextPrimary,
            AutoSize = true,
            Anchor = AnchorStyles.Top | AnchorStyles.Right,
            TextAlign = ContentAlignment.MiddleRight
        };
        amount.Location = new Point(row.Width - amount.PreferredWidth - 24, 18);

        var divider = new Panel
        {
            BackColor = AppTheme.CardBorder,
            Height = 1,
            Dock = DockStyle.Bottom
        };

        row.Controls.Add(store);
        row.Controls.Add(date);
        row.Controls.Add(amount);
        row.Controls.Add(divider);
        return row;
    }
}
