using System.Globalization;
using receipt_project_front.Models;
using receipt_project_front.Services;
using receipt_project_front.UI;

namespace receipt_project_front.Pages;

public partial class ReceiptBrowserPage : UserControl, IRefreshablePage
{
    private List<ReceiptSummary> _receipts = new();
    private int _index;
    private CancellationTokenSource? _imageCts;
    private bool _loadingList;

    public ReceiptBrowserPage()
    {
        InitializeComponent();
        WireInlineEdits();
        ApplyEmptyState("표시할 영수증이 없습니다");
    }

    private void WireInlineEdits()
    {
        EnableInlineEdit(categoryLabel,
            getValue: () => Current?.Category ?? Current?.AiSuggestedCategory ?? string.Empty,
            commit: async newValue => await UpdateCategoryAsync(newValue),
            minWidth: 160);

        EnableInlineEdit(storeNameLabel,
            getValue: () => Current?.StoreName ?? string.Empty,
            commit: newValue =>
            {
                ReplaceCurrent(c => c with { StoreName = newValue });
                storeNameLabel.Text = newValue;
                return Task.CompletedTask;
            },
            minWidth: 200);

        EnableInlineEdit(dateLabel,
            getValue: () => (Current?.PurchasedAt ?? Current?.CreatedAt)?.LocalDateTime.ToString("yyyy-MM-dd HH:mm") ?? string.Empty,
            commit: newValue =>
            {
                if (DateTimeOffset.TryParseExact(newValue, "yyyy-MM-dd HH:mm",
                        CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out var parsed))
                {
                    ReplaceCurrent(c => c with { PurchasedAt = parsed });
                    dateLabel.Text = parsed.LocalDateTime.ToString("yyyy-MM-dd HH:mm");
                }
                return Task.CompletedTask;
            },
            minWidth: 200);

        EnableInlineEdit(amountLabel,
            getValue: () => Current?.Amount?.ToString("0.##", CultureInfo.InvariantCulture) ?? string.Empty,
            commit: newValue =>
            {
                var cleaned = newValue.Replace(",", string.Empty).Trim();
                if (decimal.TryParse(cleaned, NumberStyles.Number, CultureInfo.InvariantCulture, out var amount))
                {
                    ReplaceCurrent(c => c with { Amount = amount });
                    amountLabel.Text = "합계 " + amount.ToString("N0", CultureInfo.InvariantCulture);
                }
                return Task.CompletedTask;
            },
            minWidth: 200);
    }

    private ReceiptSummary? Current =>
        _receipts.Count == 0 ? null : _receipts[_index];

    private void ReplaceCurrent(Func<ReceiptSummary, ReceiptSummary> mutate)
    {
        if (_receipts.Count == 0) return;
        _receipts[_index] = mutate(_receipts[_index]);
    }

    private async Task UpdateCategoryAsync(string newCategory)
    {
        var current = Current;
        if (current is null) return;

        try
        {
            await ReceiptApi.ConfirmCategoryAsync(current.ReceiptId, newCategory);
            ReplaceCurrent(c => c with { Category = newCategory });
            categoryLabel.Text = newCategory;
        }
        catch (ApiException ex)
        {
            MessageBox.Show(ex.Message, "카테고리 저장 실패",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"카테고리 저장 실패: {ex.Message}", "카테고리 저장 실패",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    public async void OnNavigatedTo()
    {
        if (_loadingList) return;

        _loadingList = true;
        ApplyLoadingState();
        try
        {
            var list = await ReceiptApi.GetListAsync();
            _receipts = list.Items.ToList();
            _index = 0;

            if (_receipts.Count == 0)
                ApplyEmptyState("표시할 영수증이 없습니다");
            else
                await ShowCurrentAsync();
        }
        catch (ApiException ex) { ApplyEmptyState(ex.Message); }
        catch (HttpRequestException ex) { ApplyEmptyState($"백엔드에 연결할 수 없습니다.\n{ex.Message}"); }
        catch (Exception ex) { ApplyEmptyState($"불러오기 실패: {ex.Message}"); }
        finally { _loadingList = false; }
    }

    private async void PrevButton_Click(object? sender, EventArgs e)
    {
        if (_receipts.Count == 0) return;
        _index = (_index - 1 + _receipts.Count) % _receipts.Count;
        await ShowCurrentAsync();
    }

    private async void NextButton_Click(object? sender, EventArgs e)
    {
        if (_receipts.Count == 0) return;
        _index = (_index + 1) % _receipts.Count;
        await ShowCurrentAsync();
    }

    private async Task ShowCurrentAsync()
    {
        _imageCts?.Cancel();
        _imageCts = new CancellationTokenSource();
        var ct = _imageCts.Token;

        var r = _receipts[_index];

        SetEditableLabelsVisible(true);
        categoryLabel.Text = r.Category ?? r.AiSuggestedCategory ?? "분류 없음";
        categoryLabel.ForeColor = AppTheme.TextPrimary;
        storeNameLabel.Text = string.IsNullOrEmpty(r.StoreName) ? "(상호명 없음)" : r.StoreName;
        storeNameLabel.ForeColor = AppTheme.TextPrimary;
        dateLabel.Text = (r.PurchasedAt ?? r.CreatedAt).LocalDateTime.ToString("yyyy-MM-dd HH:mm");
        amountLabel.Text = r.Amount.HasValue
            ? "합계 " + r.Amount.Value.ToString("N0", CultureInfo.InvariantCulture)
            : "합계 -";

        prevButton.Enabled = _receipts.Count > 1;
        nextButton.Enabled = _receipts.Count > 1;

        photoBox.Image?.Dispose();
        photoBox.Image = null;

        try
        {
            var image = await ReceiptApi.GetImageAsync(r.ReceiptId, ct);
            if (ct.IsCancellationRequested)
            {
                image.Dispose();
                return;
            }
            photoBox.Image = image;
        }
        catch (OperationCanceledException) { }
        catch
        {
            photoBox.Image = null;
        }
    }

    private void ApplyLoadingState()
    {
        SetEditableLabelsVisible(false);
        categoryLabel.Text = "불러오는 중...";
        categoryLabel.ForeColor = AppTheme.TextSecondary;
        categoryLabel.Visible = true;
        dateLabel.Text = string.Empty;
        amountLabel.Text = string.Empty;
        prevButton.Enabled = false;
        nextButton.Enabled = false;
        photoBox.Image?.Dispose();
        photoBox.Image = null;
    }

    private void ApplyEmptyState(string message)
    {
        SetEditableLabelsVisible(false);
        categoryLabel.Text = message;
        categoryLabel.ForeColor = AppTheme.TextSecondary;
        categoryLabel.Visible = true;
        dateLabel.Text = string.Empty;
        amountLabel.Text = "-";
        prevButton.Enabled = false;
        nextButton.Enabled = false;
        photoBox.Image?.Dispose();
        photoBox.Image = null;
    }

    private void SetEditableLabelsVisible(bool visible)
    {
        // Hide store/separator while in empty/loading states so they don't
        // float around with stale text.
        storeNameLabel.Visible = visible;
        separatorLabel.Visible = visible;
    }

    // Inline-edit overlay: click a Label to swap in a TextBox at the same
    // bounds. Enter / focus-loss commits, Esc cancels.
    private static void EnableInlineEdit(
        Label label,
        Func<string> getValue,
        Func<string, Task> commit,
        int minWidth = 120)
    {
        label.Cursor = Cursors.IBeam;
        label.Click += (_, _) => StartEdit(label, getValue, commit, minWidth);
    }

    private static void StartEdit(
        Label label,
        Func<string> getValue,
        Func<string, Task> commit,
        int minWidth)
    {
        if (label.Parent is null) return;

        var current = getValue();

        var textbox = new TextBox
        {
            Text = current,
            Font = label.Font,
            BorderStyle = BorderStyle.FixedSingle,
            BackColor = Color.White,
            ForeColor = AppTheme.TextPrimary
        };

        var parent = label.Parent;
        var bounds = label.Bounds;
        textbox.Location = bounds.Location;
        textbox.Size = new Size(
            Math.Max(bounds.Width, minWidth),
            Math.Max(bounds.Height, textbox.PreferredHeight));

        parent.Controls.Add(textbox);
        textbox.BringToFront();
        label.Visible = false;

        textbox.Focus();
        textbox.SelectAll();

        var resolved = false;

        async Task Commit()
        {
            if (resolved) return;
            resolved = true;
            var newValue = textbox.Text.Trim();
            Cleanup();
            if (!string.IsNullOrEmpty(newValue) && newValue != current)
                await commit(newValue);
        }

        void Cancel()
        {
            if (resolved) return;
            resolved = true;
            Cleanup();
        }

        void Cleanup()
        {
            parent.Controls.Remove(textbox);
            textbox.Dispose();
            label.Visible = true;
        }

        textbox.KeyDown += async (_, e) =>
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                await Commit();
            }
            else if (e.KeyCode == Keys.Escape)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                Cancel();
            }
        };
        textbox.Leave += async (_, _) => await Commit();
    }
}
