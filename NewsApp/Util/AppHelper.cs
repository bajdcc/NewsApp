using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace NewsApp.Util
{
    public class AppHelper
    {
        [DllImport("user32", CharSet = CharSet.Unicode)]
        static public extern IntPtr FindWindow(string cls, string win);
        [DllImport("user32")]
        static public extern IntPtr SetForegroundWindow(IntPtr hWnd);
        [DllImport("user32")]
        static public extern bool IsIconic(IntPtr hWnd);
        [DllImport("user32")]
        static public extern bool OpenIcon(IntPtr hWnd);
    }
}
