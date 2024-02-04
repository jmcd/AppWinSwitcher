namespace AppWinSwitcher;

public partial class MainForm : Form
{
    private readonly NotifyIcon _notifyIcon;

    public MainForm()
    {
        InitializeComponent();

        _notifyIcon = SetupSystrayGuff();
    }

    public Action? OnRequestDefineShortcut { get; set; }

    private NotifyIcon SetupSystrayGuff()
    {
        var notifyIcon = new NotifyIcon();
        notifyIcon.Icon = Icon;
        notifyIcon.BalloonTipText = "Hello systray!";
        
        Resize += (_, _) =>
        {
            if (WindowState != FormWindowState.Minimized)
            {
                return;
            }

            notifyIcon.Visible = true;
            notifyIcon.ShowBalloonTip(3000);
            ShowInTaskbar = false;
        };


        notifyIcon.Click += (_, _) =>
        {
            WindowState = FormWindowState.Normal;
            ShowInTaskbar = true;
            notifyIcon.Visible = false;
        };

        return notifyIcon;
    }
    

    private void btnDefineShortcut_Click(object sender, EventArgs e)
    {
        OnRequestDefineShortcut?.Invoke();
    }

    public void ModelWasChanged(Mode mode, ISet<WinUser.VirtualKeys> shortcut)
    {
        lblShortcutText.Text = mode switch
        {
            Mode.Defining => "Press shortcut keys now to define",
            Mode.Listening => shortcut.Any()
                ? "Now listening for shortcut keys: " + string.Join(", ", shortcut.Order().Select(k => k.ToString()))
                : "Shortcut not defined",
            _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null)
        };

        btnDefineShortcut.Enabled = mode != Mode.Defining;
    }
}