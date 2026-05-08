using receipt_project_front.Pages;
using receipt_project_front.Services;
using receipt_project_front.UI;

namespace receipt_project_front;

public partial class MainForm : Form
{
    private readonly Dictionary<NavItem, UserControl> _pages = new();
    private readonly Dictionary<NavItem, Button> _navButtons = new();
    private NavItem _activeItem;

    public bool SignOutRequested { get; private set; }

    public MainForm()
    {
        InitializeComponent();
        WireUpPages();
        Navigate(NavItem.Overview);
    }

    private void WireUpPages()
    {
        var upload = new UploadPage { Dock = DockStyle.Fill };
        var edit = new EditPage { Dock = DockStyle.Fill };

        _pages[NavItem.Upload] = upload;
        _pages[NavItem.Overview] = new OverviewPage { Dock = DockStyle.Fill };
        _pages[NavItem.Browser] = new ReceiptBrowserPage { Dock = DockStyle.Fill };
        _pages[NavItem.Analytics] = new AnalyticsPage { Dock = DockStyle.Fill };
        _pages[NavItem.Settings] = new SettingsPage { Dock = DockStyle.Fill };

        // Edit is not a sidebar item; it's the post-upload state shown while
        // 영수증 업로드 stays highlighted.
        _pages[NavItem.Edit] = edit;

        upload.UploadCompleted += (_, _) => Navigate(NavItem.Edit, highlight: NavItem.Upload);
        edit.SaveCompleted += (_, _) => Navigate(NavItem.Overview);

        if (_pages[NavItem.Settings] is SettingsPage settings)
        {
            settings.SignOutRequested += (_, _) =>
            {
                SignOutRequested = true;
                AppState.Current.Clear();
                Close();
            };
        }
    }

    public void Navigate(NavItem item, NavItem? highlight = null)
    {
        if (!_pages.TryGetValue(item, out var page)) return;

        contentPanel.SuspendLayout();
        contentPanel.Controls.Clear();
        contentPanel.Controls.Add(page);
        contentPanel.ResumeLayout();

        var newActive = highlight ?? item;
        if (_navButtons.TryGetValue(_activeItem, out var prevBtn))
            StyleNavButton(prevBtn, active: false);

        _activeItem = newActive;
        if (_navButtons.TryGetValue(newActive, out var nextBtn))
            StyleNavButton(nextBtn, active: true);

        if (page is IRefreshablePage refreshable)
            refreshable.OnNavigatedTo();
    }

    private static void StyleNavButton(Button button, bool active)
    {
        button.BackColor = active ? AppTheme.SidebarActive : AppTheme.SidebarBg;
        button.ForeColor = active ? Color.White : AppTheme.SidebarText;
        button.Font = active
            ? new Font(AppTheme.FontFamily, 10.5f, FontStyle.Bold)
            : new Font(AppTheme.FontFamily, 10.5f, FontStyle.Regular);
    }
}

public enum NavItem
{
    Upload,
    Overview,
    Browser,
    Analytics,
    Settings,
    Edit
}

public interface IRefreshablePage
{
    void OnNavigatedTo();
}
