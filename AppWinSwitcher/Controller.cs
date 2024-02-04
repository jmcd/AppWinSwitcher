namespace AppWinSwitcher;

public class Controller : IDisposable
{
    private static readonly WinUser.VirtualKeys[] ModifierVirtualKeys =
    [
        WinUser.VirtualKeys.VK_LMENU, WinUser.VirtualKeys.VK_RMENU, // AKA Alt
        WinUser.VirtualKeys.VK_LCONTROL, WinUser.VirtualKeys.VK_RCONTROL,
        WinUser.VirtualKeys.VK_LSHIFT, WinUser.VirtualKeys.VK_RSHIFT,
        WinUser.VirtualKeys.VK_LWIN, WinUser.VirtualKeys.VK_RWIN
    ];

    private readonly MainForm _mainForm;
    private readonly KeyboardWindowsHook _kbd;

    private readonly Switcher _switcher;

    private Mode _mode = Mode.Listening;
    private ISet<WinUser.VirtualKeys> _shortcut = new HashSet<WinUser.VirtualKeys>();

    public Controller(MainForm mainForm)
    {
        _kbd = new KeyboardWindowsHook(OnKeys);
        _switcher = new Switcher();
        _mainForm = mainForm;

        _mainForm.OnRequestDefineShortcut = () =>
        {
            _mode = Mode.Defining;
            SignalModelChanged();
        };

        SignalModelChanged();
    }

    public void Dispose()
    {
        _kbd.Dispose();
    }

    private void SignalModelChanged()
    {
        _mainForm.ModelWasChanged(_mode, _shortcut);
    }

    private void OnKeys(bool didChange, ISet<WinUser.VirtualKeys> oldKeys, ISet<WinUser.VirtualKeys> newKeys)
    {
        switch (_mode)
        {
            case Mode.Defining:
                DidDefineShortcut(newKeys);
                
                break;
            case Mode.Listening:

                if (!ShortcutIsValid())
                {
                    break;
                }

                if (DidFinishSwitching(oldKeys, newKeys))
                {
                    break;
                }

                DidSwitch(newKeys);
                
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private bool DidDefineShortcut(ISet<WinUser.VirtualKeys> newKeys)
    {
        var modifierCount = newKeys.Count(u => ModifierVirtualKeys.Contains(u));
        var otherCount = newKeys.Count - modifierCount;

        var enoughKeysPressedToDefineShortcut = modifierCount > 0 && otherCount > 0;
        if (!enoughKeysPressedToDefineShortcut)
        {
            return false;
        }

        _shortcut = newKeys.ToHashSet();
        _mode = Mode.Listening;
        SignalModelChanged();
        
        return true;
    }

    private bool ShortcutIsValid()
    {
        return _shortcut.Count > 0;
    }
    
    private bool DidFinishSwitching(ISet<WinUser.VirtualKeys> oldKeys, ISet<WinUser.VirtualKeys> newKeys)
    {
        var keysThatWereReleased = oldKeys.Except(newKeys);
        var modifierWasReleased = keysThatWereReleased.Any(WinUser.VirtualKeysUtil.IsModifier);

        var userHasFinishedSwitching = modifierWasReleased;
        if (!userHasFinishedSwitching)
        {
            return false;
        }

        _switcher.CleanupAfterSwitchingFinished();
        return true;

    }

    private bool DidSwitch(ISet<WinUser.VirtualKeys> newKeys)
    {
        var userHasPressedShortcutKeys = newKeys.SetEquals(_shortcut);
        if (!userHasPressedShortcutKeys)
        {
            return false;
        }

        _switcher.Switch();
        return true;
    }
}