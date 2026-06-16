using System.ComponentModel;
using System.Globalization;
using receipt_project_front.Models;
using receipt_project_front.Services;
using receipt_project_front.UI;

namespace receipt_project_front.Pages;

public partial class ReceiptBrowserPage : UserControl, IRefreshablePage
{
    private const string AllMonths = "전체";

    private List<ReceiptSummary> _receipts = new();   // 전체 로드분
    private Guid? _selectedId;                          // 현재 선택된 영수증
    private bool _loadingList;
    private bool _populatingMonths;
    private CancellationTokenSource? _imageCts;

    // 다른 페이지(예: Overview)에서 특정 영수증을 열도록 요청할 때 설정한다.
    // 런타임 전용 속성이므로 디자이너 직렬화에서 제외한다(WFO1000).
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Guid? PendingReceiptId { get; set; }

    public ReceiptBrowserPage()
    {
        InitializeComponent();
        WireInlineEdits();
        ApplyEmptyDetail("표시할 영수증이 없습니다");
    }

    // ───────────────────────── 인라인 수정 ─────────────────────────

    private void WireInlineEdits()
    {
        EnableInlineEdit(categoryLabel,
            getValue: () => Current?.Category ?? Current?.AiSuggestedCategory ?? string.Empty,
            commit: async newValue => await UpdateCategoryAsync(newValue),
            minWidth: 160);

        EnableInlineEdit(storeNameLabel,
            getValue: () => Current?.StoreName ?? string.Empty,
            commit: async newValue => await UpdateStoreNameAsync(newValue),
            minWidth: 200);

        EnableInlineEdit(dateLabel,
            getValue: () => (Current?.PurchasedAt ?? Current?.CreatedAt)?.LocalDateTime.ToString("yyyy-MM-dd HH:mm") ?? string.Empty,
            commit: async newValue =>
            {
                if (DateTimeOffset.TryParseExact(newValue, "yyyy-MM-dd HH:mm",
                        CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out var parsed))
                    await UpdateDateAsync(parsed);
                else
                    ShowSaveError("날짜", "형식은 yyyy-MM-dd HH:mm 입니다.");
            },
            minWidth: 200);

        EnableInlineEdit(amountLabel,
            getValue: () => Current?.Amount?.ToString("0.##", CultureInfo.InvariantCulture) ?? string.Empty,
            commit: async newValue =>
            {
                var cleaned = newValue.Replace(",", string.Empty).Trim();
                if (decimal.TryParse(cleaned, NumberStyles.Number, CultureInfo.InvariantCulture, out var amount))
                    await UpdateAmountAsync(amount);
                else
                    ShowSaveError("금액", "숫자만 입력해 주세요.");
            },
            minWidth: 200);
    }

    private ReceiptSummary? Current =>
        _selectedId is { } id ? _receipts.FirstOrDefault(r => r.ReceiptId == id) : null;

    private void ReplaceCurrent(Func<ReceiptSummary, ReceiptSummary> mutate)
    {
        if (_selectedId is not { } id) return;
        var idx = _receipts.FindIndex(r => r.ReceiptId == id);
        if (idx < 0) return;
        _receipts[idx] = mutate(_receipts[idx]);
        RenderList(FilteredReceipts()); // 목록 행 텍스트도 갱신
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
        catch (Exception ex) { ShowSaveError("카테고리", ex.Message); }
    }

    private async Task UpdateStoreNameAsync(string newName)
    {
        var current = Current;
        if (current is null) return;

        try
        {
            await ReceiptApi.UpdateAsync(current.ReceiptId, new UpdateReceiptRequest(StoreName: newName));
            ReplaceCurrent(c => c with { StoreName = newName });
            storeNameLabel.Text = newName;
        }
        catch (Exception ex) { ShowSaveError("상호명", ex.Message); }
    }

    private async Task UpdateDateAsync(DateTimeOffset purchasedAt)
    {
        var current = Current;
        if (current is null) return;

        try
        {
            await ReceiptApi.UpdateAsync(current.ReceiptId, new UpdateReceiptRequest(PurchasedAt: purchasedAt));
            ReplaceCurrent(c => c with { PurchasedAt = purchasedAt });
            dateLabel.Text = purchasedAt.LocalDateTime.ToString("yyyy-MM-dd HH:mm");
        }
        catch (Exception ex) { ShowSaveError("날짜", ex.Message); }
    }

    private async Task UpdateAmountAsync(decimal amount)
    {
        var current = Current;
        if (current is null) return;

        try
        {
            await ReceiptApi.UpdateAsync(current.ReceiptId, new UpdateReceiptRequest(Amount: amount));
            ReplaceCurrent(c => c with { Amount = amount });
            amountLabel.Text = "합계 " + amount.ToString("N0", CultureInfo.InvariantCulture);
        }
        catch (Exception ex) { ShowSaveError("금액", ex.Message); }
    }

    private static void ShowSaveError(string field, string message) =>
        MessageBox.Show($"{field} 저장 실패: {message}", "저장 실패",
            MessageBoxButtons.OK, MessageBoxIcon.Warning);

    // ───────────────────────── 로드 / 월 필터 ─────────────────────────

    public async void OnNavigatedTo()
    {
        if (_loadingList) return;

        _loadingList = true;
        ApplyLoadingState();
        try
        {
            var list = await ReceiptApi.GetListAsync(page: 1, pageSize: 100);
            _receipts = list.Items.ToList();

            // Overview 등에서 특정 영수증을 지정했으면 그 영수증/월을 선택한다.
            string? preferMonth = null;
            if (PendingReceiptId is { } targetId &&
                _receipts.FirstOrDefault(r => r.ReceiptId == targetId) is { } target)
            {
                _selectedId = targetId;
                preferMonth = MonthKey(target);
            }
            else
            {
                _selectedId = null;
            }
            PendingReceiptId = null;

            PopulateMonths(preferMonth);
            await ApplyMonthFilterAsync();
        }
        catch (ApiException ex) { ClearWithMessage(ex.Message); }
        catch (HttpRequestException ex) { ClearWithMessage($"백엔드에 연결할 수 없습니다.\n{ex.Message}"); }
        catch (Exception ex) { ClearWithMessage($"불러오기 실패: {ex.Message}"); }
        finally { _loadingList = false; }
    }

    private void PopulateMonths(string? preferMonth)
    {
        _populatingMonths = true;
        try
        {
            monthSelector.Items.Clear();
            monthSelector.Items.Add(AllMonths);
            foreach (var m in _receipts.Select(MonthKey).Distinct().OrderByDescending(m => m))
                monthSelector.Items.Add(m);

            var idx = preferMonth is not null ? monthSelector.Items.IndexOf(preferMonth) : -1;
            monthSelector.SelectedIndex = idx >= 0 ? idx : 0; // 못 찾으면 전체
        }
        finally { _populatingMonths = false; }
    }

    private async void MonthSelector_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (_populatingMonths || _loadingList) return;
        await ApplyMonthFilterAsync();
    }

    private List<ReceiptSummary> FilteredReceipts()
    {
        IEnumerable<ReceiptSummary> q = _receipts;
        if (monthSelector.SelectedItem is string month && month != AllMonths)
            q = q.Where(r => MonthKey(r) == month);
        return q.OrderByDescending(r => r.PurchasedAt ?? r.CreatedAt).ToList();
    }

    private async Task ApplyMonthFilterAsync()
    {
        var filtered = FilteredReceipts();

        // 현재 선택이 목록에 없으면 첫(최신) 항목으로 보정.
        if (_selectedId is null || filtered.All(r => r.ReceiptId != _selectedId))
            _selectedId = filtered.Count > 0 ? filtered[0].ReceiptId : (Guid?)null;

        RenderList(filtered);

        if (Current is null)
            ApplyEmptyDetail("표시할 영수증이 없습니다");
        else
            await ShowCurrentAsync();
    }

    private static string MonthKey(ReceiptSummary r)
    {
        var d = (r.PurchasedAt ?? r.CreatedAt).LocalDateTime;
        return $"{d.Year}-{d.Month:D2}";
    }

    // ───────────────────────── 목록 렌더 ─────────────────────────

    private void RenderList(List<ReceiptSummary> items)
    {
        listPanel.SuspendLayout();
        listPanel.Controls.Clear();

        if (items.Count == 0)
        {
            listPanel.Controls.Add(new Label
            {
                Text = "영수증이 없습니다",
                Dock = DockStyle.Top,
                Height = 56,
                Font = AppTheme.Body,
                ForeColor = AppTheme.TextSecondary,
                TextAlign = ContentAlignment.MiddleCenter
            });
        }
        else
        {
            // Dock=Top은 마지막에 추가된 행이 맨 위로 오므로,
            // (최신→과거) 목록을 역순으로 추가해 최신이 맨 위에 오게 한다.
            foreach (var r in Enumerable.Reverse(items))
                listPanel.Controls.Add(BuildRow(r));
        }

        listPanel.ResumeLayout();
    }

    private Panel BuildRow(ReceiptSummary r)
    {
        var selected = r.ReceiptId == _selectedId;

        var row = new Panel
        {
            Height = 60,
            Dock = DockStyle.Top,
            Cursor = Cursors.Hand,
            BackColor = selected ? AppTheme.ContentBg : AppTheme.CardBg
        };

        var store = new Label
        {
            Text = string.IsNullOrWhiteSpace(r.StoreName) ? "(이름 없음)" : r.StoreName,
            Font = AppTheme.BodyBold,
            ForeColor = AppTheme.TextPrimary,
            AutoSize = true,
            Location = new Point(16, 9)
        };

        var amount = new Label
        {
            Text = r.Amount.HasValue ? "₩" + r.Amount.Value.ToString("N0", CultureInfo.InvariantCulture) : "-",
            Font = AppTheme.BodyBold,
            ForeColor = AppTheme.TextPrimary,
            AutoSize = true,
            Anchor = AnchorStyles.Top | AnchorStyles.Right
        };

        var date = new Label
        {
            Text = (r.PurchasedAt ?? r.CreatedAt).LocalDateTime.ToString("yyyy-MM-dd"),
            Font = AppTheme.Caption,
            ForeColor = AppTheme.TextSecondary,
            AutoSize = true,
            Location = new Point(16, 33)
        };

        var category = new Label
        {
            Text = r.Category ?? r.AiSuggestedCategory ?? "분류 없음",
            Font = AppTheme.Caption,
            ForeColor = AppTheme.TextSecondary,
            AutoSize = true,
            Anchor = AnchorStyles.Top | AnchorStyles.Right
        };

        var divider = new Panel { BackColor = AppTheme.CardBorder, Height = 1, Dock = DockStyle.Bottom };

        row.Controls.Add(store);
        row.Controls.Add(amount);
        row.Controls.Add(date);
        row.Controls.Add(category);
        row.Controls.Add(divider);

        void Reflow()
        {
            amount.Left = Math.Max(16, row.ClientSize.Width - amount.PreferredWidth - 16);
            amount.Top = 9;
            category.Left = Math.Max(16, row.ClientSize.Width - category.PreferredWidth - 16);
            category.Top = 33;
        }
        row.Resize += (_, _) => Reflow();
        Reflow();

        void OnClick(object? s, EventArgs e) => _ = SelectAsync(r.ReceiptId);
        row.Click += OnClick;
        foreach (Control child in row.Controls)
        {
            child.Cursor = Cursors.Hand;
            child.Click += OnClick;
        }

        return row;
    }

    private async Task SelectAsync(Guid id)
    {
        if (_selectedId == id) return;
        _selectedId = id;
        RenderList(FilteredReceipts()); // 선택 하이라이트 갱신
        await ShowCurrentAsync();
    }

    // ───────────────────────── 상세 / 삭제 ─────────────────────────

    private async void DeleteButton_Click(object? sender, EventArgs e)
    {
        var current = Current;
        if (current is null) return;

        var name = string.IsNullOrEmpty(current.StoreName) ? "이 영수증" : $"\"{current.StoreName}\"";
        var confirm = MessageBox.Show(
            $"{name}을(를) 삭제할까요?\n삭제하면 되돌릴 수 없습니다.",
            "영수증 삭제",
            MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
        if (confirm != DialogResult.Yes) return;

        deleteButton.Enabled = false;
        try
        {
            await ReceiptApi.DeleteAsync(current.ReceiptId);

            var preserveMonth = monthSelector.SelectedItem as string;
            _receipts.RemoveAll(r => r.ReceiptId == current.ReceiptId);
            _selectedId = null; // ApplyMonthFilter가 인접 영수증을 다시 선택

            PopulateMonths(preserveMonth); // 그 달의 마지막 영수증이면 월 항목도 사라짐
            await ApplyMonthFilterAsync();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"삭제 실패: {ex.Message}", "삭제 실패",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            deleteButton.Enabled = Current is not null;
        }
    }

    private async Task ShowCurrentAsync()
    {
        var r = Current;
        if (r is null)
        {
            ApplyEmptyDetail("표시할 영수증이 없습니다");
            return;
        }

        _imageCts?.Cancel();
        _imageCts = new CancellationTokenSource();
        var ct = _imageCts.Token;

        SetEditableLabelsVisible(true);
        categoryLabel.Text = r.Category ?? r.AiSuggestedCategory ?? "분류 없음";
        categoryLabel.ForeColor = AppTheme.TextPrimary;
        storeNameLabel.Text = string.IsNullOrEmpty(r.StoreName) ? "(상호명 없음)" : r.StoreName;
        storeNameLabel.ForeColor = AppTheme.TextPrimary;
        dateLabel.Text = (r.PurchasedAt ?? r.CreatedAt).LocalDateTime.ToString("yyyy-MM-dd HH:mm");
        amountLabel.Text = r.Amount.HasValue
            ? "합계 " + r.Amount.Value.ToString("N0", CultureInfo.InvariantCulture)
            : "합계 -";
        deleteButton.Enabled = true;

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
        listPanel.Controls.Clear();
        listPanel.Controls.Add(new Label
        {
            Text = "불러오는 중...",
            Dock = DockStyle.Top,
            Height = 56,
            Font = AppTheme.Body,
            ForeColor = AppTheme.TextSecondary,
            TextAlign = ContentAlignment.MiddleCenter
        });
        ApplyEmptyDetail(string.Empty);
    }

    // 목록까지 모두 비우고 상세에 메시지를 표시(로드 실패용).
    private void ClearWithMessage(string message)
    {
        _receipts = new();
        _selectedId = null;
        RenderList(new List<ReceiptSummary>());
        ApplyEmptyDetail(message);
    }

    // 상세 패널만 빈/메시지 상태로.
    private void ApplyEmptyDetail(string message)
    {
        SetEditableLabelsVisible(false);
        categoryLabel.Text = message;
        categoryLabel.ForeColor = AppTheme.TextSecondary;
        categoryLabel.Visible = true;
        dateLabel.Text = string.Empty;
        amountLabel.Text = "-";
        deleteButton.Enabled = false;
        photoBox.Image?.Dispose();
        photoBox.Image = null;
    }

    private void SetEditableLabelsVisible(bool visible)
    {
        storeNameLabel.Visible = visible;
        separatorLabel.Visible = visible;
    }

    // ───────────────────────── 인라인 편집 오버레이 ─────────────────────────
    // Label을 클릭하면 같은 위치에 TextBox를 띄운다. Enter/포커스 해제 = 커밋, Esc = 취소.

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
