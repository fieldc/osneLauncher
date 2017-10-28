using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace osnE.WindowsHooks
{
    enum GetWindow_Cmd : uint
    {
        GW_HWNDFIRST = 0,
        GW_HWNDLAST = 1,
        GW_HWNDNEXT = 2,
        GW_HWNDPREV = 3,
        GW_OWNER = 4,
        GW_CHILD = 5,
        GW_ENABLEDPOPUP = 6
    }
    public class WindowInfo :IComparable<WindowInfo>
    {
        public string WindowName { get; set; }
        public IntPtr hwnd { get; set; }
        public int lParam { get; set; }
        public WindowInfo(string name, IntPtr hwnd, int lParam)
        {
            this.WindowName = name;
            this.hwnd = hwnd;
            this.lParam = lParam;
        }

        int IComparable<WindowInfo>.CompareTo(WindowInfo other)
        {
            return this.WindowName.CompareTo(other.WindowName);
        }
        public bool StartsWith(string s)
        {
            if (string.IsNullOrEmpty(s))
                return true;

            return this.WindowName.StartsWith(s, StringComparison.OrdinalIgnoreCase);
        }
        public bool Contains(string s)
        {
            if (string.IsNullOrEmpty(s))
                return true;
            return this.WindowName.ToLower().Contains(s.ToLower());
        }

    }
    public class WindowEnumerator
    {
        public static List<WindowInfo> WindowList = new List<WindowInfo>();

        public static List<WindowInfo> EnumerateWindows()
        {
            lock (WindowList)
            {
                WindowList.Clear();
                NativeMethods.EnumDesktopWindowsDelegate filter = EnumerateWindowsCallback;
                if (NativeMethods.EnumDesktopWindows(IntPtr.Zero, filter, IntPtr.Zero))
                {
                    return WindowList;
                }
            }
            return new List<WindowInfo>();
        }

        public static bool EnumerateWindowsCallback(IntPtr hWnd, int lParam)
        {
            StringBuilder strbTitle = new StringBuilder(255);
            int nLength = NativeMethods.GetWindowText(hWnd, strbTitle, strbTitle.Capacity + 1);
            string strTitle = strbTitle.ToString();
            if (NativeMethods.IsWindowVisible(hWnd) && string.IsNullOrEmpty(strTitle) == false)
            {
                if (NativeMethods.GetParent(hWnd) == (IntPtr)0)
                {
                    if (NativeMethods.GetWindow(hWnd, GetWindow_Cmd.GW_OWNER) == (IntPtr)0)
                    {
                        WindowList.Add(new WindowInfo(strbTitle.ToString(), hWnd, lParam));
                    }
                }
            }
            return true;
        }

        public static void ForceWindowIntoForeground(IntPtr hWnd)
        {
            uint currentThread = NativeMethods.GetCurrentThreadId();

            IntPtr activeWindow = NativeMethods.GetForegroundWindow();
            uint activeProcess;
            uint activeThread = NativeMethods.GetWindowThreadProcessId(activeWindow, out activeProcess);

            uint windowProcess;
            uint windowThread = NativeMethods.GetWindowThreadProcessId(hWnd, out windowProcess);

            if (currentThread != activeThread)
                NativeMethods.AttachThreadInput(currentThread, activeThread, true);
            if (windowThread != currentThread)
                NativeMethods.AttachThreadInput(windowThread, currentThread, true);

            uint oldTimeout = 0, newTimeout = 0;
            NativeMethods.SystemParametersInfo(NativeMethods.SPI.SPI_GETFOREGROUNDLOCKTIMEOUT, 0, ref oldTimeout, NativeMethods.SPIF.None);
            NativeMethods.SystemParametersInfo(NativeMethods.SPI.SPI_GETFOREGROUNDLOCKTIMEOUT, 0, ref newTimeout, NativeMethods.SPIF.None);
            NativeMethods.LockSetForegroundWindow(NativeMethods.LSFW_UNLOCK);
            NativeMethods.AllowSetForegroundWindow(NativeMethods.ASFW_ANY);

            NativeMethods.SetForegroundWindow(hWnd);

            NativeMethods.WINDOWPLACEMENT w;
            NativeMethods.GetWindowPlacement(hWnd, out w);
            switch (w.ShowCmd)
            {
                case NativeMethods.ShowWindowCommands.ShowMaximized:
                    NativeMethods.ShowWindow(hWnd, NativeMethods.ShowWindowCommands.ShowMaximized);
                    break;
                case NativeMethods.ShowWindowCommands.Normal:
                    NativeMethods.ShowWindow(hWnd, NativeMethods.ShowWindowCommands.Normal);
                    break;
                case NativeMethods.ShowWindowCommands.ShowMinimized:
                    NativeMethods.ShowWindow(hWnd, NativeMethods.ShowWindowCommands.Restore);
                    //ShowWindow(hWnd, ShowWindowCommands.Normal);
                    break;
                default:
                    NativeMethods.ShowWindow(hWnd, NativeMethods.ShowWindowCommands.Normal);
                    Thread.Sleep(250);
                    break;

            }

            NativeMethods.SystemParametersInfo(NativeMethods.SPI.SPI_SETFOREGROUNDLOCKTIMEOUT, 0, ref oldTimeout, 0);

            if (currentThread != activeThread)
                NativeMethods.AttachThreadInput(currentThread, activeThread, false);
            if (windowThread != currentThread)
                NativeMethods.AttachThreadInput(windowThread, currentThread, false);
        }
    }
}
