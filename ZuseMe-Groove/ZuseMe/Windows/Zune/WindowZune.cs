using ArnoldVinkCode;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using static ArnoldVinkCode.AVInteropDll;

namespace ZuseMe.Windows
{
    public class WindowZune
    {
        //Window variables
        private IntPtr windowHandle;

        //Window process message
        public IntPtr WindowProcessMessage(IntPtr hWnd, IntPtr uMsg, IntPtr wParam, IntPtr lParam)
        {
            try
            {
                if (uMsg == 74)
                {
                    COPYDATASTRUCT cds = (COPYDATASTRUCT)Marshal.PtrToStructure(lParam, typeof(COPYDATASTRUCT));
                    string msg = Marshal.PtrToStringUni(cds.lpData, cds.cbData);

                    if (msg.StartsWith("ZUNE"))
                    {
                        string[] msgSplit = msg.Split(new string[] { "\\0" }, StringSplitOptions.None);
                        if (msgSplit.Length >= 7)
                        {
                            AppVariables.ZuneArtist = msgSplit[5];
                            AppVariables.ZuneAlbum = msgSplit[6];
                            AppVariables.ZuneTitle = msgSplit[4];
                        }
                    }
                }
                return DefWindowProc(hWnd, uMsg, wParam, lParam);
            }
            catch { }
            return IntPtr.Zero;
        }

        //Window show
        public void Show()
        {
            AVActions.TaskStartBackground(delegate
            {
                //Create window class
                WindowClassEx windowClassEx = new WindowClassEx
                {
                    cbSize = (uint)Marshal.SizeOf(typeof(WindowClassEx)),
                    lpfnWndProc = WindowProcessMessage,
                    lpszMenuName = string.Empty,
                    lpszClassName = "MsnMsgrUIManager",
                    hIconSm = IntPtr.Zero
                };

                //Register window class
                RegisterClassEx(ref windowClassEx);

                //Create window
                WindowStyles windowStyles = WindowStyles.WS_DISABLED;
                WindowStylesEx windowStylesEx = WindowStylesEx.WS_EX_TOOLWINDOW;
                windowHandle = CreateWindowEx(windowStylesEx, windowClassEx.lpszClassName, windowClassEx.lpszMenuName, windowStyles, 0, 0, 0, 0, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);

                //Update window
                UpdateWindow(windowHandle);

                Debug.WriteLine("Zune receive window opened: " + windowHandle);

                //Get message loop
                while (windowHandle != IntPtr.Zero)
                {
                    GetMessage(out _, IntPtr.Zero, 0, 0);
                }
            });
        }
    }
}