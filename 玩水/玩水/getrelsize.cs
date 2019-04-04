using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace 玩水
{
    class Getrelsize
    {
        #region Win32 API
        [DllImport("user32.dll")]
        static extern IntPtr GetDC(IntPtr ptr);
        [DllImport("gdi32.dll")]
        static extern int GetDeviceCaps(
        IntPtr hdc, // handle to DC
        int nIndex // index of capability
        );
        [DllImport("user32.dll", EntryPoint = "ReleaseDC")]
        static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDc);
        #endregion
        #region DeviceCaps常量
        const int DESKTOPVERTRES = 117;
        const int DESKTOPHORZRES = 118;
        #endregion
        public Size Getsize()
        {
            IntPtr hdc = GetDC(IntPtr.Zero);
            Size size = new Size();
            size.Width = GetDeviceCaps(hdc, DESKTOPHORZRES);
            size.Height = GetDeviceCaps(hdc, DESKTOPVERTRES);
            return size;
        }
        public double Getw()
        {
            IntPtr hdc = GetDC(IntPtr.Zero);
            Size size = new Size();
            size.Width = GetDeviceCaps(hdc, DESKTOPHORZRES);
            size.Height = GetDeviceCaps(hdc, DESKTOPVERTRES);
            return (double)size.Width;
        }

    }
}
