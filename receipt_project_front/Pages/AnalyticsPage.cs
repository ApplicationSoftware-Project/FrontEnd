using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using receipt_project_front.Models;
using receipt_project_front.Services;
using receipt_project_front.UI;

namespace receipt_project_front.Pages;

public partial class AnalyticsPage : UserControl, IRefreshablePage
{
    private enum View { Monthly, Daily, Category }
    private View _view = View.Monthly;
    private bool _loading;
    private bool _populatingMonths;
    private IReadOnlyList<ReceiptSummary>? _cachedCategoryList;

    public AnalyticsPage()
    {
        InitializeComponent();
        monthlyLink.LinkClicked += (_, _) => SwitchView(View.Monthly);
        dailyLink.LinkClicked += (_, _) => SwitchView(View.Daily);
        categoryLink.LinkClicked += (_, _) => SwitchView(View.Category);
        monthSelector.SelectedIndexChanged += MonthSelector_SelectedIndexChanged;
    }

    public async void OnNavigatedTo() => await LoadAsync();

    private void SwitchView(View view)
    {
        if (_view == view || _loading) return;
        _view = view;
        UpdateTabStyle();
        _ = LoadAsync();
    }

    private void UpdateTabStyle()
    {
        monthlyLink.LinkColor = _view == View.Monthly ? AppTheme.Accent : AppTheme.TextSecondary;
        dailyLink.LinkColor = _view == View.Daily ? AppTheme.Accent : AppTheme.TextSecondary;
        categoryLink.LinkColor = _view == View.Category ? AppTheme.Accent : AppTheme.TextSecondary;
        categoryHeader.Visible = _view == View.Category;
    }

    private void MonthSelector_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (_loading || _populatingMonths) return;
        if (_cachedCategoryList is null) return;
        if (monthSelector.SelectedItem is not string selectedMonth) return;
        try
        {
            DrawCategoryChart(_cachedCategoryList, selectedMonth);
        }
        catch (Exception ex) { ShowStatus($"차트 오류: {ex.Message}"); }
    }

    private async Task LoadAsync()
    {
        if (_loading) return;
        _loading = true;
        ShowStatus("불러오는 중...");
        try
        {
            if (_view == View.Monthly)
                await LoadMonthlyAsync();
            else if (_view == View.Daily)
                await LoadDailyAsync();
            else
                await LoadCategoryAsync();
        }
        catch (ApiException ex) { ShowStatus(ex.Message); }
        catch (HttpRequestException ex) { ShowStatus($"백엔드에 연결할 수 없습니다.\n{ex.Message}"); }
        catch (Exception ex) { ShowStatus($"불러오기 실패: {ex.Message}"); }
        finally { _loading = false; }
    }

    private async Task LoadMonthlyAsync()
    {
        var trend = await ReceiptApi.GetMonthlyTrendAsync();
        if (trend.Count == 0) { ShowStatus("데이터가 없습니다."); return; }

        var labels = trend.Select(t => $"{t.Year}-{t.Month:D2}").ToArray();
        var values = trend.Select(t => t.TotalAmount).ToArray();

        cartesianChart.Series = new ISeries[]
        {
            new ColumnSeries<double>
            {
                Name = "월별 지출",
                Values = values,
                Fill = new SolidColorPaint(new SKColor(59, 130, 246))
            }
        };
        cartesianChart.XAxes = new Axis[]
        {
            new Axis { Labels = labels, LabelsRotation = 15 }
        };
        cartesianChart.YAxes = new Axis[]
        {
            new Axis { Labeler = v => "₩" + ((long)Math.Max(v, 0)).ToString("N0") }
        };
        ShowChart();
    }

    private async Task LoadDailyAsync()
    {
        var list = await ReceiptApi.GetListAsync(page: 1, pageSize: 200);
        var now = DateTime.Now;

        var daily = list.Items
            .Where(r => r.Amount.HasValue)
            .Select(r => (Day: (r.PurchasedAt ?? r.CreatedAt).LocalDateTime, Amount: r.Amount!.Value))
            .Where(x => x.Day.Year == now.Year && x.Day.Month == now.Month)
            .GroupBy(x => x.Day.Day)
            .OrderBy(g => g.Key)
            .ToList();

        if (daily.Count == 0) { ShowStatus($"{now.Year}년 {now.Month}월 데이터가 없습니다."); return; }

        var labels = daily.Select(g => $"{g.Key}일").ToArray();
        var values = daily.Select(g => (double)g.Sum(x => x.Amount)).ToArray();

        cartesianChart.Series = new ISeries[]
        {
            new ColumnSeries<double>
            {
                Name = "일별 지출",
                Values = values,
                Fill = new SolidColorPaint(new SKColor(34, 197, 94))
            }
        };
        cartesianChart.XAxes = new Axis[]
        {
            new Axis { Labels = labels }
        };
        cartesianChart.YAxes = new Axis[]
        {
            new Axis { Labeler = v => "₩" + ((long)Math.Max(v, 0)).ToString("N0") }
        };
        ShowChart();
    }

    private async Task LoadCategoryAsync()
    {
        // TODO: pageSize=100이므로 영수증이 100건을 초과하면 일부 데이터가 누락됩니다. 추후 전체 페이지네이션 필요.
        var list = await ReceiptApi.GetListAsync(page: 1, pageSize: 100);
        _cachedCategoryList = list.Items;

        var months = list.Items
            .Select(r => (r.PurchasedAt ?? r.CreatedAt).LocalDateTime)
            .Select(d => $"{d.Year}-{d.Month:D2}")
            .Distinct()
            .OrderByDescending(m => m)
            .ToList();

        // 콤보 채우는 동안 SelectedIndexChanged 재진입을 막는다
        _populatingMonths = true;
        try
        {
            monthSelector.Items.Clear();
            foreach (var m in months)
                monthSelector.Items.Add(m);
            if (monthSelector.Items.Count > 0)
                monthSelector.SelectedIndex = 0;
        }
        finally
        {
            _populatingMonths = false;
        }

        if (monthSelector.SelectedItem is not string selected)
        {
            ShowStatus("데이터가 없습니다.");
            return;
        }

        DrawCategoryChart(list.Items, selected);
    }

    private void DrawCategoryChart(IReadOnlyList<ReceiptSummary> items, string selectedMonth)
    {
        var parts = selectedMonth.Split('-');
        if (parts.Length != 2
            || !int.TryParse(parts[0], out int year)
            || !int.TryParse(parts[1], out int month))
        {
            ShowStatus("월 형식 오류.");
            return;
        }

        // LocalDateTime 기준으로 필터(UTC 월 밀림 회피), Category null/빈문자 → "미분류"
        var grouped = items
            .Where(r => r.Amount.HasValue)
            .Select(r => (
                Date: (r.PurchasedAt ?? r.CreatedAt).LocalDateTime,
                Amount: r.Amount!.Value,
                Category: string.IsNullOrWhiteSpace(r.Category) ? "미분류" : r.Category
            ))
            .Where(x => x.Date.Year == year && x.Date.Month == month)
            .GroupBy(x => x.Category)
            .Select(g => (Category: g.Key, Total: (double)g.Sum(x => x.Amount)))
            .OrderByDescending(x => x.Total)
            .ToList();

        if (grouped.Count == 0)
        {
            ShowStatus($"{selectedMonth} 데이터가 없습니다.");
            return;
        }

        var labels = grouped.Select(g => g.Category).ToArray();
        var values = grouped.Select(g => g.Total).ToArray();

        cartesianChart.Series = new ISeries[]
        {
            new ColumnSeries<double>
            {
                Name = "카테고리별 지출",
                Values = values,
                Fill = new SolidColorPaint(new SKColor(168, 85, 247))
            }
        };
        cartesianChart.XAxes = new Axis[]
        {
            new Axis { Labels = labels, LabelsRotation = 15 }
        };
        cartesianChart.YAxes = new Axis[]
        {
            new Axis { Labeler = v => "₩" + ((long)Math.Max(v, 0)).ToString("N0") }
        };
        ShowChart();
    }

    private void ShowStatus(string message)
    {
        cartesianChart.Visible = false;
        statusLabel.Text = message;
        statusLabel.Visible = true;
    }

    private void ShowChart()
    {
        statusLabel.Visible = false;
        cartesianChart.Visible = true;
    }
}
