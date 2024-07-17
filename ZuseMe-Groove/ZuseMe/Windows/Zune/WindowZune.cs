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
        private bool windowOpen = false;

        //Window process message
        public IntPtr WindowProcessMessage(IntPtr hWnd, uint uMsg, IntPtr wParam, IntPtr lParam)
        {
            try
            {
                if (uMsg == 74)
                {
                    COPYDATASTRUCT cds = (COPYDATASTRUCT)Marshal.PtrToStructure(lParam, typeof(COPYDATASTRUCT));
                    string msg = Marshal.PtrToStringUni(cds.lpData, cds.cbData);

                    if (msg.StartsWith("ZUNE"))
                    {
                        string[] Split = msg.Split(new string[] { "\\0" }, StringSplitOptions.None);
                        if (Split.Length >= 7)
                        {
                            string ZuneTitle = Split[4];
                            string ZuneArtist = Split[5];
                            string ZuneAlbum = Split[6];
                            AppVariables.ZuneArtist = ZuneArtist;
                            AppVariables.ZuneAlbum = ZuneAlbum;
                            AppVariables.ZuneTitle = ZuneTitle;
                        }
                    }
                }
                return DefWindowProc(hWnd, uMsg, wParam, lParam);
            }
            catch { }
            return IntPtr.Zero;
        }

        //Window create
        public void Show()
        {
            AVActions.TaskStartBackground(delegate
            {
                //Create window class
                WNDCLASSEX windowClassEx = new WNDCLASSEX
                {
                    cbSize = WNDCLASSEX.classSize,
                    style = 0,
                    lpfnWndProc = WindowProcessMessage,
                    cbClsExtra = 0,
                    cbWndExtra = 0,
                    hInstance = IntPtr.Zero,
                    hIcon = IntPtr.Zero,
                    hCursor = IntPtr.Zero,
                    hbrBackground = IntPtr.Zero,
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

                // Our process would run in Background
                UpdateWindow(windowHandle);

                Debug.WriteLine("Zune receive window opened: " + windowHandle);

                windowOpen = true;
                while (windowOpen)
                {
                    GetMessage(out _, IntPtr.Zero, 0, 0);
                }
            });
        }
    }
}