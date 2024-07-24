using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace WrathBlueprintTree
{
    public class KeyEventArgs : EventArgs
    {
        public int KeyCode { get; }

        public KeyEventArgs(int keyCode)
        {
            KeyCode = keyCode;
        }
    }

    public class GlobalKeyboardListener : IDisposable
    {
        private bool _disposed = false;
        private readonly HookProc? _hookProc;
        private IntPtr _hookID = IntPtr.Zero;
        public event EventHandler<KeyEventArgs>? KeyboardKeyPressed;
        public event EventHandler<KeyEventArgs>? KeyboardKeyReleased;

        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_KEYUP = 0x0101;
        //private const int WM_SYSKEYDOWN = 0x0104; //SYSKEY was added because the 'Alt' keydown is hijacked on a system level and does not pass through
        //private const int WM_SYSKEYUP = 0x0105; // SYSKEY has the side effect of not respecting application focus, and MAUI does not either.
            // So to use Alt-Down ANOTHER workaround would be required to catch and expose window.LostFocus and window.GetFocus events
            //If i actually NEED Alt-Down, I'll look into it further. Shift/Ctrl should suffice.

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        public GlobalKeyboardListener()
        {
            _hookProc = HookCallback;
            _hookID = SetHook(_hookProc);
        }

        private IntPtr SetHook(HookProc proc)
        {
            using (var curProcess = Process.GetCurrentProcess())
            using (var curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private delegate IntPtr HookProc(int nCode, IntPtr wParam, IntPtr lParam);

        private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                int vkCode = Marshal.ReadInt32(lParam);
                //if (wParam == (IntPtr)WM_KEYDOWN || wParam == (IntPtr)WM_SYSKEYDOWN)
                if (wParam == (IntPtr)WM_KEYDOWN)
                {
                    OnKeyboardKeyPressed(vkCode);
                }
                //else if (wParam == (IntPtr)WM_KEYUP || wParam == (IntPtr)WM_SYSKEYUP)
                else if (wParam == (IntPtr)WM_KEYUP)
                {
                    OnKeyboardKeyReleased(vkCode);
                }
            }
            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

         protected virtual void OnKeyboardKeyPressed(int keyCode)
        {
           KeyboardKeyPressed?.Invoke(this, new KeyEventArgs(keyCode));
        }

        protected virtual void OnKeyboardKeyReleased(int keyCode)
        {
            KeyboardKeyReleased?.Invoke(this, new KeyEventArgs(keyCode));
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
        if (_disposed)
            return;

        if (disposing)
        {
            // Dispose managed resources here.
            // Example: Unhook the keyboard listener
            if (_hookID != IntPtr.Zero)
            {
                UnhookWindowsHookEx(_hookID);
                _hookID = IntPtr.Zero;
            }
        }

        // Dispose unmanaged resources here.

        _disposed = true;
    }

    ~GlobalKeyboardListener()
    {
        Dispose(false);
    }
   
    }
}