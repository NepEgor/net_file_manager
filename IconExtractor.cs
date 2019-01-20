using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace directories
{
    class IconExtractor
    {
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct SHFILEINFO
        {
            public IntPtr hIcon;
            public int iIcon;
            public uint dwAttributes;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szDisplayName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
            public string szTypeName;
        }

        [DllImport("shell32.dll")]
        public static extern UIntPtr SHGetFileInfo(
            string pszPath,
            uint dwFileAttributes,
            ref SHFILEINFO psfi,
            uint cbFileInfo,
            uint uFlags);

        public static Icon Extract(string path)
        {
            Icon icon;

            SHFILEINFO sfi = new SHFILEINFO();
            var ret = SHGetFileInfo(path, 0, ref sfi, (uint)Marshal.SizeOf(sfi), 0x100); // SHGFI_ICON (0x000000100)

            if (ret.ToUInt32() == 0) return null;

            icon = Icon.FromHandle(sfi.hIcon);

            return icon;
        }
    }
}
