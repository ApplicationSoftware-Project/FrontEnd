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
    private enum View { Monthly, Daily }
    private View _view = View.Monthly;
    private bool _loading;

    public AnalyticsPage()
    {
        InitializeComponent();
        monthlyLink.LinkClicked += (_, _) => SwitchView(View.Monthly);
        dailyLink.LinkClicked += (_, _) => SwitchView(View.Daily);
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
            else
                await LoadDailyAsync();
        }
        catch (ApiException ex) { ShowStatus(ex.Message); }
        catch (HttpRequestException ex) { ShowStatus($"백엔드에 연결할 수 없습니다.\n{ex.Message}"); }
        catch (Exception ex) { ShowStatus($"불러오기 실패: {ex.Message}"); }
        finally { _loading = false; }
    }

    private async Task LoadMonthlyAsync()
    {
        var list = await ReceiptApi.GetListAsync(page: 1, pageSize: 100);

        // 백엔드 monthly-trend는 UTC 기준으로 월을 집계해 월초 영수증이 전월로 밀린다
        // (예: 2026-05-01 → 4월). 일별 분석과 동일하게 LocalDateTime 기준으로 직접 집계한다.
        var monthly = list.Items
            .Where(r => r.Amount.HasValue)
            .Select(r => (Date: (r.PurchasedAt ?? r.CreatedAt).LocalDateTime, Amount: r.Amount!.Value))
            .GroupBy(x => new { x.Date.Year, x.Date.Month })
            .OrderBy(g => g.Key.Year).ThenBy(g => g.Key.Month)
            .ToList();

        if (monthly.Count == 0) { ShowStatus("데이터가 없습니다."); return; }

        var labels = monthly.Select(g => $"{g.Key.Year}-{g.Key.Month:D2}").ToArray();
        var values = monthly.Select(g => (double)g.Sum(x => x.Amount)).ToArray();

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
        var list = await ReceiptApi.GetListAsync(page: 1, pageSize: 100);
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
