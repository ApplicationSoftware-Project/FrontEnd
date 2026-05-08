using System.Drawing;

namespace receipt_project_front.UI;

internal static class AppTheme
{
    public static readonly Color SidebarBg = Color.FromArgb(30, 41, 59);
    public static readonly Color SidebarBgAlt = Color.FromArgb(15, 23, 42);
    public static readonly Color SidebarText = Color.FromArgb(226, 232, 240);
    public static readonly Color SidebarTextDim = Color.FromArgb(148, 163, 184);
    public static readonly Color SidebarHover = Color.FromArgb(51, 65, 85);
    public static readonly Color SidebarActive = Color.FromArgb(59, 130, 246);

    public static readonly Color ContentBg = Color.FromArgb(248, 250, 252);
    public static readonly Color CardBg = Color.White;
    public static readonly Color CardBorder = Color.FromArgb(226, 232, 240);

    public static readonly Color TextPrimary = Color.FromArgb(15, 23, 42);
    public static readonly Color TextSecondary = Color.FromArgb(100, 116, 139);
    public static readonly Color TextMuted = Color.FromArgb(148, 163, 184);

    public static readonly Color Accent = Color.FromArgb(59, 130, 246);
    public static readonly Color AccentHover = Color.FromArgb(37, 99, 235);
    public static readonly Color Danger = Color.FromArgb(239, 68, 68);
    public static readonly Color Success = Color.FromArgb(34, 197, 94);

    public const string FontFamily = "Segoe UI";
    public const string DisplayFontFamily = "Impact";

    public static Font H1 => new(FontFamily, 22f, FontStyle.Bold);
    public static Font H2 => new(FontFamily, 16f, FontStyle.Bold);
    public static Font H3 => new(FontFamily, 13f, FontStyle.Bold);
    public static Font Body => new(FontFamily, 10f);
    public static Font BodyBold => new(FontFamily, 10f, FontStyle.Bold);
    public static Font Caption => new(FontFamily, 9f);
    public static Font Display => new(DisplayFontFamily, 56f, FontStyle.Regular);
}
