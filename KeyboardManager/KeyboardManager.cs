using System;
using System.Windows.Input;
using System.Windows;
using System.Diagnostics;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using osnE.Interop;
using WindowsInput;
using WindowsInput.Native;

namespace osnE.WindowsHooks
{
        public class KeyboardManager : IDisposable
        {
            private InputSimulator _inputSimulator = new InputSimulator();
            private object _currentClipboardLock = new object { };
            private IDataObject currentClipboard = new DataObject();
            private volatile bool activated = false;
            private volatile bool triggeredCapslock = false;
            private NativeMethods.LowLevelKeyboardProc _proc;
            private IntPtr _hookID = IntPtr.Zero;

            public event EventHandler Activated;
            protected void OnActivated()
            {
                if(this.Activated!=null)
                {
                    this.Activated(this, new EventArgs());
                }
            }

            public event EventHandler Deactivated;
            protected void OnDeactivated()
            {
                if (this.Deactivated != null)
                {
                    this.Deactivated(this, new EventArgs());
                }
            }

            public delegate void KeystrokeCapturedEventHandler(object sender, KeystrokeCapturedEventArgs k);
            public event KeystrokeCapturedEventHandler KeystrokeCaptured;
            protected void OnKeystrokeCaptured(Key k)
            {
                if(this.KeystrokeCaptured!=null)
                {
                    Console.WriteLine("KB MGR ThreadId=" + System.Threading.Thread.CurrentThread.ManagedThreadId.ToString());
                    foreach (KeystrokeCapturedEventHandler d in KeystrokeCaptured.GetInvocationList())
                    {
                        d.BeginInvoke(this, new KeystrokeCapturedEventArgs(k, Keyboard.Modifiers), new AsyncCallback(delegate { }), new object { });
                    }
                }
            }

            public KeyboardManager()
            {
                _proc = this.HookCallback;
                _hookID = this.SetHook(this._proc);
                
            }
            
            public void Deactivate()
            {
                this.activated = false;
            }

            public void CapslockOff()
            {
                if (_inputSimulator.InputDeviceState.IsTogglingKeyInEffect(VirtualKeyCode.CAPITAL))
                {
                    triggeredCapslock = true;
                    _inputSimulator.Keyboard.KeyPress(VirtualKeyCode.CAPITAL);
                }
            }
            public void CapslockOn()
            {
                if (!_inputSimulator.InputDeviceState.IsTogglingKeyInEffect(VirtualKeyCode.CAPITAL))
                {
                    triggeredCapslock = true;
                    _inputSimulator.Keyboard.KeyPress(VirtualKeyCode.CAPITAL);
                }
            }
            private IntPtr SetHook(NativeMethods.LowLevelKeyboardProc proc)
            {
                using (Process curProcess = Process.GetCurrentProcess())
                {
                    using (ProcessModule curModule = curProcess.MainModule)
                    {
                        return NativeMethods.SetWindowsHookEx(NativeMethods.WH_KEYBOARD_LL, proc, NativeMethods.GetModuleHandle(curModule.ModuleName), 0);
                    }
                }
            }

            private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
            {
                if (nCode >= 0 && wParam == (IntPtr)NativeMethods.WM_KEYDOWN)
                {
                    int vkCode = Marshal.ReadInt32(lParam);
                    Key pressed = KeyInterop.KeyFromVirtualKey(vkCode);
                    Console.WriteLine("[HookCallback]:KeyPressedEvent=" + pressed.ToString() + " vkCode=" + vkCode.ToString());
                    if (pressed == Key.Escape)
                    {
                        //this.RestoreClipboard();
                        Console.WriteLine("Caught escape ACTIVATED=" + activated.ToString());
                        if (activated == true)
                        {
                            this.activated = false;
                            this.OnDeactivated();
                            return (IntPtr)1;
                        }
                    }
                    else if (pressed == Key.CapsLock || pressed == Key.Capital)
                    {
                        if (activated == false)
                        {
                            IDataObject tmpClipboard = new DataObject() ;
                            lock (this._currentClipboardLock)
                            {
                                tmpClipboard = Clipboard.GetDataObject();
                                _inputSimulator.Keyboard.ModifiedKeyStroke(VirtualKeyCode.CONTROL, VirtualKeyCode.VK_C);
                                Thread.Sleep(200);
                                try
                                {
                                    this.currentClipboard = Clipboard.GetDataObject();
                                }
                                catch (Exception ex) { Console.WriteLine("Exception while capturing clipboard ex=" + ex.Message); }
                            }
                            
                            this.activated = true;
                            this.OnActivated();
                        }
                        if (!triggeredCapslock)
                            return (IntPtr)1;

                        triggeredCapslock = false;
                    }
                    else if(this.activated)
                    {
                        this.OnKeystrokeCaptured(pressed);
                        return (IntPtr)1;
                    }
                }
                return NativeMethods.CallNextHookEx(_hookID, nCode, wParam, lParam);
            }

            public void Dispose()
            {
                NativeMethods.UnhookWindowsHookEx(this._hookID);
            }

            public IDataObject CurrentClipboard
            {
                get { return currentClipboard; }
                set { currentClipboard = value; }
            }
        }

}
