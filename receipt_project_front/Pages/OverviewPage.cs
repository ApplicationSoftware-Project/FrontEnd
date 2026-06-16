using System.Globalization;
using receipt_project_front.Models;
using receipt_project_front.Services;
using receipt_project_front.UI;

namespace receipt_project_front.Pages;

public partial class OverviewPage : UserControl, IRefreshablePage
{
    // 최근 영수증 행을 클릭하면 해당 영수증 ID를 전달한다. MainForm이 구독해
    // 영수증 조회 페이지로 이동시킨다.
    public event EventHandler<Guid>? ReceiptSelected;

    public OverviewPage()
    {
        InitializeComponent();
        UpdateMonthCaption();
        SetMonthlyExpense(0m);
        SetRecentReceipts(Array.Empty<ReceiptSummary>());
    }

    // "이번 달 지출" 카드의 캡션을 현재 연/월(예: "2026년 6월")로 설정한다.
    private void UpdateMonthCaption()
    {
        var now = DateTime.Now;
        monthlyCaptionLabel.Text = $"{now.Year}년 {now.Month}월";
    }

    public async void OnNavigatedTo()
    {
        UpdateMonthCaption();
        SetMonthlyExpense(0m);
        SetRecentReceipts(Array.Empty<ReceiptSummary>());
        try
        {
            var list = await ReceiptApi.GetListAsync(page: 1, pageSize: 100);
            var now = DateTime.Now;

            // 이번 달 지출: 구매일/업로드일(로컬) 기준으로 이번 달 합산.
            // (백엔드 monthly-trend는 UTC 기준이라 월초 영수증이 전월로 밀려 월별/일별 화면과 어긋난다.)
            var monthlyTotal = list.Items
                .Where(r => r.Amount.HasValue)
                .Select(r => (Date: (r.PurchasedAt ?? r.CreatedAt).LocalDateTime, Amount: r.Amount!.Value))
                .Where(x => x.Date.Year == now.Year && x.Date.Month == now.Month)
                .Sum(x => x.Amount);
            SetMonthlyExpense(monthlyTotal);

            // 최근 영수증 (구매일/업로드일 기준 내림차순). SetRecentReceipts가 5건으로 제한한다.
            var recent = list.Items
                .OrderByDescending(r => r.PurchasedAt ?? r.CreatedAt)
                .ToList();
            SetRecentReceipts(recent);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[Overview] 데이터를 불러오지 못했습니다: {ex.Message}");
        }
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
            // receipts는 최신→과거 순. Dock=Top은 마지막에 추가된 행이 맨 위에
            // 오므로, 역순(과거→최신)으로 추가해 최신 영수증이 맨 위에 오게 한다.
            foreach (var r in receipts.Take(5).Reverse())
                receiptsListPanel.Controls.Add(BuildRow(r));
        }

        receiptsListPanel.ResumeLayout();
    }

    private Panel BuildRow(ReceiptSummary r)
    {
        var row = new Panel { Height = 56, Dock = DockStyle.Top, Padding = new Padding(16, 12, 16, 12), Cursor = Cursors.Hand };

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

        // 행 전체(빈 영역 + 라벨들)를 클릭하면 해당 영수증을 조회 페이지에서 연다.
        void OnRowClick(object? sender, EventArgs e) => ReceiptSelected?.Invoke(this, r.ReceiptId);
        row.Click += OnRowClick;
        foreach (Control child in row.Controls)
        {
            child.Cursor = Cursors.Hand;
            child.Click += OnRowClick;
        }

        return row;
    }
}
