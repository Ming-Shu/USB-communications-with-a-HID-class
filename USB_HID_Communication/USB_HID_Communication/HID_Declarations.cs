using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Win32.SafeHandles;//This document describes the state of SafeHandles and CriticalFinalizerObject and their supporting requirements in the Mono Runtime.
using System.Runtime.InteropServices; //StructLayout
namespace USB_HID_Communication
{
    ///  <summary>
    ///  Supports Windows API functions for accessing HID-class USB devices.
    ///  Includes routines for retrieving information about the configuring 
    ///  a HID and sending and receiving reports via control and interrupt transfers. 
    ///  </summary>
    internal sealed partial class HID
    {
        internal static class Native_Methods
        {
            //  from hidpi.h
            //  Typedef enum defines a set of integer constants for HidP_Report_Type

            internal const Int16 HidP_Input = 0;
            internal const Int16 HidP_Output = 1;
            internal const Int16 HidP_Feature = 2;

            [StructLayout(LayoutKind.Sequential)]
            internal struct HIDD_ATTRIBUTES
            {
                internal Int32 Size;
                internal UInt16 VendorID;
                internal UInt16 ProductID;
                internal UInt16 VersionNumber;
            }

            internal struct HIDP_CAPS
            {
                internal Int16 Usage;
                internal Int16 UsagePage;
                internal Int16 InputReportByteLength;
                internal Int16 OutputReportByteLength;
                internal Int16 FeatureReportByteLength;
                [MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)]
                internal Int16[] Reserved;
                internal Int16 NumberLinkCollectionNodes;
                internal Int16 NumberInputButtonCaps;
                internal Int16 NumberInputValueCaps;
                internal Int16 NumberInputDataIndices;
                internal Int16 NumberOutputButtonCaps;
                internal Int16 NumberOutputValueCaps;
                internal Int16 NumberOutputDataIndices;
                internal Int16 NumberFeatureButtonCaps;
                internal Int16 NumberFeatureValueCaps;
                internal Int16 NumberFeatureDataIndices;
            }

            //  If IsRange is false, UsageMin is the Usage and UsageMax is unused.
            //  If IsStringRange is false, StringMin is the String index and StringMax is unused.
            //  If IsDesignatorRange is false, DesignatorMin is the designator index and DesignatorMax is unused.

            [DllImport("hid.dll", SetLastError = true)]
            internal static extern Boolean HidD_FlushQueue(SafeFileHandle HidDeviceObject);

            [DllImport("hid.dll", SetLastError = true)]
            internal static extern Boolean HidD_FreePreparsedData(IntPtr PreparsedData);//frees the buffer reserved by HidD_GetPreparsedData.

            [DllImport("hid.dll", SetLastError = true)]
            internal static extern Boolean HidD_GetAttributes(SafeFileHandle HidDeviceObject, ref HIDD_ATTRIBUTES Attributes);//Get the Vendor ID,Product ID,Product Version Number.

            [DllImport("hid.dll", SetLastError = true)]
            internal static extern Boolean HidD_GetFeature(SafeFileHandle HidDeviceObject, Byte[] lpReportBuffer, Int32 ReportBufferLength);

            [DllImport("hid.dll", SetLastError = true)]
            internal static extern Boolean HidD_GetInputReport(SafeFileHandle HidDeviceObject, Byte[] lpReportBuffer, Int32 ReportBufferLength);

            [DllImport("hid.dll", SetLastError = true)]
            internal static extern void HidD_GetHidGuid(ref Guid HidGuid);

            [DllImport("hid.dll", SetLastError = true)]
            internal static extern Boolean HidD_GetNumInputBuffers(SafeFileHandle HidDeviceObject, ref Int32 NumberBuffers);

            [DllImport("hid.dll", SetLastError = true)]
            internal static extern Boolean HidD_GetPreparsedData(SafeFileHandle HidDeviceObject, ref IntPtr PreparsedData);//retrieves a 'pointer to a buffer' containing information about the device's capabilities.

            [DllImport("hid.dll", SetLastError = true)]
            internal static extern Boolean HidD_SetFeature(SafeFileHandle HidDeviceObject, Byte[] lpReportBuffer, Int32 ReportBufferLength);

            [DllImport("hid.dll", SetLastError = true)]
            internal static extern Boolean HidD_SetNumInputBuffers(SafeFileHandle HidDeviceObject, Int32 NumberBuffers);

            [DllImport("hid.dll", SetLastError = true)]
            internal static extern Boolean HidD_SetOutputReport(SafeFileHandle HidDeviceObject, Byte[] lpReportBuffer, Int32 ReportBufferLength);

            [DllImport("hid.dll", SetLastError = true)]
            internal static extern Int32 HidP_GetCaps(IntPtr PreparsedData, ref HIDP_CAPS Capabilities);//find out a device's capabilities.

            [DllImport("hid.dll", SetLastError = true)]
            internal static extern Int32 HidP_GetValueCaps(Int32 ReportType, Byte[] ValueCaps, ref Int32 ValueCapsLength, IntPtr PreparsedData);

        }
    }
}
