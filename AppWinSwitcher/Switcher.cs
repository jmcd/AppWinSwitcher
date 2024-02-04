using Windows.Win32;
using Windows.Win32.Foundation;

namespace AppWinSwitcher;

public class Switcher 
{
    private HWND[]? _hwndsForCurrentApplication;
    
    public void CleanupAfterSwitchingFinished()
    {
        _hwndsForCurrentApplication = null;
    }

    public void Switch()
    {
        var hwndFg = PInvoke.GetForegroundWindow();
        if (hwndFg == HWND.Null)
        {
            return;
        }

        _hwndsForCurrentApplication ??= GetHwndsForApplication(hwndFg).ToArray();
        if (_hwndsForCurrentApplication.Length < 2)
        {
            return;
        }

        var hwndFgIndex = Array.IndexOf(_hwndsForCurrentApplication, hwndFg);
        var hwndNextIndex = (hwndFgIndex + 1) % _hwndsForCurrentApplication.Length;
        var hwndNext = _hwndsForCurrentApplication[hwndNextIndex];

        PInvoke.SetForegroundWindow(hwndNext);
    }

    private static IEnumerable<HWND> GetHwndsForApplication(HWND hwndFg)
    {
        return WinUtils.TryGetProcessMainModuleForValidWindow(hwndFg, out var mmFg)
            ? WinUtils.EnumWindows().Where(hwnd => HwndHasMatchingMainModule(hwnd, mmFg.FileName))
            : Enumerable.Empty<HWND>();
    }

    private static bool HwndHasMatchingMainModule(HWND hwnd, string mainModuleFileName)
    {
        return WinUtils.TryGetProcessMainModuleForValidWindow(hwnd, out var mm) &&
               mm.FileName.Equals(mainModuleFileName, StringComparison.InvariantCultureIgnoreCase);
    }
}