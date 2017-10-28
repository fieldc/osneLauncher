using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Runtime.InteropServices;

namespace osnE.WindowsHooks
{
    public class ExplorerHooks
    {
        internal enum MOUSEEVENTF : uint
        {
            MOUSEEVENTF_ABSOLUTE = 0x8000,
            MOUSEEVENTF_HWHEEL = 0x01000,
            MOUSEEVENTF_MOVE = 0x0001,
            MOUSEEVENTF_MOVE_NOCOALESCE = 0x2000,
            MOUSEEVENTF_LEFTDOWN = 0x0002,
            MOUSEEVENTF_LEFTUP = 0x0004,
            MOUSEEVENTF_RIGHTDOWN = 0x0008,
            MOUSEEVENTF_RIGHTUP = 0x0010,
            MOUSEEVENTF_MIDDLEDOWN = 0x0020,
            MOUSEEVENTF_MIDDLEUP = 0x0040,
            MOUSEEVENTF_VIRTUALDESK = 0x4000,
            MOUSEEVENTF_WHEEL = 0x0800,
            MOUSEEVENTF_XDOWN = 0x0080,
            MOUSEEVENTF_XUP = 0x0100
        }
        internal enum KEYEVENTF : uint
        {
            KEYEVENTF_EXTENDEDKEY = 0x0001,
            KEYEVENTF_KEYUP = 0x0002,
            KEYEVENTF_SCANCODE = 0x0008,
            KEYEVENTF_UNICODE = 0x0004
        }
        
        [StructLayout(LayoutKind.Explicit)]
        internal struct INPUT
        {
            [FieldOffset(0)]
            internal uint type;
#if WIN64
            [FieldOffset(8)]
#else
            [FieldOffset(4)]
#endif
            internal InputUnion U;
            internal static int Size
            {
                get { return Marshal.SizeOf(typeof(INPUT)); }
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct KEYBDINPUT
        {
            internal ushort wVk;
            internal ushort wScan;
            internal KEYEVENTF dwFlags;
            internal int time;
            internal UIntPtr dwExtraInfo;
        }
        [StructLayout(LayoutKind.Sequential)]
        internal struct MOUSEINPUT
        {
            internal int dx;
            internal int dy;
            internal uint mouseData;
            internal MOUSEEVENTF dwFlags;
            internal uint time;
            internal UIntPtr dwExtraInfo;
        }
        [StructLayout(LayoutKind.Sequential)]
        internal struct HARDWAREINPUT
        {
            internal int uMsg;
            internal short wParamL;
            internal short wParamH;
        }
       
        [StructLayout(LayoutKind.Explicit)]
        internal struct InputUnion
        {
            [FieldOffset(0)]
            internal MOUSEINPUT mi;
            [FieldOffset(0)]
            internal KEYBDINPUT ki;
            [FieldOffset(0)]
            internal HARDWAREINPUT hi;
        }

        [DllImport("user32.dll")]
        internal static extern uint SendInput(uint nInputs, [MarshalAs(UnmanagedType.LPArray), In] INPUT[] pInputs, int cbSize);


        public static void SimulateCopy()
        {
            uint response = 0;
            INPUT[] inputs = new INPUT[4];
            inputs[0].U.ki.wScan = 17;
            inputs[0].U.ki.dwFlags = 0 | KEYEVENTF.KEYEVENTF_SCANCODE;
            inputs[0].U.ki.dwExtraInfo = UIntPtr.Zero;
            inputs[0].U.ki.time = 0;
            
            inputs[1].U.ki.wScan = 67;
            inputs[1].U.ki.dwFlags = 0 | KEYEVENTF.KEYEVENTF_SCANCODE;
            inputs[1].U.ki.dwExtraInfo = UIntPtr.Zero;
            inputs[1].U.ki.time = 0;

            inputs[2].U.ki.wScan = 67;
            inputs[2].U.ki.dwFlags = KEYEVENTF.KEYEVENTF_KEYUP | KEYEVENTF.KEYEVENTF_SCANCODE;
            inputs[2].U.ki.dwExtraInfo = UIntPtr.Zero;
            inputs[2].U.ki.time = 0;

            inputs[3].U.ki.wScan = 17;
            inputs[3].U.ki.dwFlags = KEYEVENTF.KEYEVENTF_KEYUP | KEYEVENTF.KEYEVENTF_SCANCODE;
            inputs[3].U.ki.dwExtraInfo = UIntPtr.Zero;
            inputs[3].U.ki.time = 0;
            response = SendInput(4, inputs, Marshal.SizeOf(typeof(INPUT)));
            Console.WriteLine("Response from SendInput=" + response.ToString());

            
        }

        

        /// <summary>The GetForegroundWindow function returns a handle to the foreground window.</summary>
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();
        public static object GetSelectedItem()
        {
            Type shellAppType = Type.GetTypeFromProgID("Shell.Application");
            Object shell = Activator.CreateInstance(shellAppType);
            Shell32.Folder folder = (Shell32.Folder)shellAppType.InvokeMember("NameSpace",
                System.Reflection.BindingFlags.InvokeMethod, null, shell, new object[] { "" });
            return new object { };
        }
    }
}
