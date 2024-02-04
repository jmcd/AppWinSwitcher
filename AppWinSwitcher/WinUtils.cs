using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Windows.Win32;
using Windows.Win32.Foundation;

namespace AppWinSwitcher;

internal static class WinUtils
{
    private static unsafe Process? GetProcessByWindowHandle(HWND hwnd)
    {
        uint processId = 0;
        var result = PInvoke.GetWindowThreadProcessId(hwnd, &processId);
        return result == 0 ? null : Process.GetProcessById((int)processId);
    }

    internal static List<HWND> EnumWindows()
    {
        var result = new List<HWND>();
        var kent = new HashSet<HWND>();

        PInvoke.EnumWindows((hwnd, _) =>
        {
            if (kent.Add(hwnd)) { result.Add(hwnd); }

            return true;
        }, 0);

        return result;
    }

    internal static unsafe string GetWindowText(HWND hwnd)
    {
        var bufferSize = PInvoke.GetWindowTextLength(hwnd) + 1;
        fixed (char* windowNameChars = new char[bufferSize])
        {
            if (PInvoke.GetWindowText(hwnd, windowNameChars, bufferSize) == 0)
            {
                var errorCode = Marshal.GetLastWin32Error();
                if (errorCode != 0) { throw new Win32Exception(errorCode); }
            }

            return new string(windowNameChars);
        }
    }

    public static bool TryGetProcessMainModuleForValidWindow(HWND hwnd, out ProcessModule mainModule)
    {
        if (IsValid(hwnd) && TryGetProcessMainModule(hwnd, out var mmFg))
        {
            mainModule = mmFg;
            return true;
        }

        mainModule = null!;
        return false;

        static bool TryGetProcessMainModule(HWND hwnd, out ProcessModule mainModule)
        {
            mainModule = default!;

            var p = GetProcessByWindowHandle(hwnd);
            if (p is null) { return false; }

            try
            {
                var mm = p.MainModule;
                if (mm is not null)
                {
                    mainModule = mm;
                    return true;
                }
            }
            catch
            {
                // don't care
            }

            return false;
        }

        static bool IsValid(HWND hwnd)
        {
            return PInvoke.IsWindowVisible(hwnd) && !PInvoke.IsIconic(hwnd);
        }
    }
}