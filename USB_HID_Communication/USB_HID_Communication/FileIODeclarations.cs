using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Win32.SafeHandles;   //SafeFileHandle
using System.Runtime.InteropServices;//DllImport

namespace USB_HID_Communication
{
    /// <summary>
    ///  Win32 API declarations relating to file I/O.
    /// </summary>
    internal class FileIO_Declarations
    {
        internal const Int32 FileShareRead = 1;
        internal const Int32 FileShareWrite = 2;
        internal const uint GenericRead = 0X80000000U;
        internal const Int32 GenericWrite = 0X40000000;
        internal const Int32 InvalidHandleValue = -1;
        internal const Int32 OpenExisting = 3;

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern SafeFileHandle CreateFile(String lpFileName, UInt32 dwDesiredAccess, Int32 dwShareMode, IntPtr lpSecurityAttributes, Int32 dwCreationDisposition, Int32 dwFlagsAndAttributes, IntPtr hTemplateFile);
    }
}
