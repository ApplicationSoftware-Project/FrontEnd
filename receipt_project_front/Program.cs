namespace receipt_project_front;

internal static class Program
{
    // Set to true to bypass the login screen and access the application directly
    public const bool LOGIN_FLAG = false;

    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();

        if (LOGIN_FLAG)
        {
            using var main = new MainForm();
            Application.Run(main);
            return;
        }

        while (true)
        {
            using var login = new LoginForm();
            if (login.ShowDialog() != DialogResult.OK)
                return;

            using var main = new MainForm();
            Application.Run(main);

            if (!main.SignOutRequested)
                return;
        }
    }
}
