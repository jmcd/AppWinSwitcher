using System.Runtime.InteropServices;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.UI.WindowsAndMessaging;

namespace AppWinSwitcher;

public class KeyboardWindowsHook : IDisposable
{
    public delegate void OnKeys(bool didChange, ISet<WinUser.VirtualKeys> oldKeys, ISet<WinUser.VirtualKeys> newKeys);

    private HHOOK? _hhook;
    private readonly HashSet<WinUser.VirtualKeys> _keysCurrentlyBeingPressed = [];
    private readonly OnKeys _onKeys;

    public KeyboardWindowsHook(OnKeys onKeys)
    {
        _onKeys = onKeys;
        _hhook = PInvoke.SetWindowsHookEx(WINDOWS_HOOK_ID.WH_KEYBOARD_LL, WindowsHookCallback, HINSTANCE.Null, 0);
    }

    ~KeyboardWindowsHook()
    {
        Unhook();
    }

    public void Dispose()
    {
        Unhook();
    }

    private void Unhook()
    {
        if (_hhook is null)
        {
            return;
        }

        PInvoke.UnhookWindowsHookEx(_hhook.Value);
        _hhook = null;
    }

    private bool KeysCurrentlyBeingPressedDidChange(bool keyWentUp, WinUser.VirtualKeys vkCode)
    {
        if (keyWentUp)
        {
            return _keysCurrentlyBeingPressed.Remove(vkCode);
        }
        
        return _keysCurrentlyBeingPressed.Add(vkCode);
    }

    private LRESULT WindowsHookCallback(int nCode, WPARAM wParam, LPARAM lParam)
    {
        var keyboardLowLevelStruct = Marshal.PtrToStructure<KBDLLHOOKSTRUCT>(lParam.Value);
        var keyWentUp = keyboardLowLevelStruct.flags.HasFlag(KBDLLHOOKSTRUCT_FLAGS.LLKHF_UP);
        var vkCode = (WinUser.VirtualKeys)keyboardLowLevelStruct.vkCode;

        var oldKeys = _keysCurrentlyBeingPressed.ToArray().ToHashSet();
        var didChange = KeysCurrentlyBeingPressedDidChange(keyWentUp, vkCode);
        var newKeys = _keysCurrentlyBeingPressed.ToArray().ToHashSet();

        _onKeys(didChange, oldKeys, newKeys);

        return PInvoke.CallNextHookEx(HHOOK.Null, nCode, wParam, lParam);
    }
}