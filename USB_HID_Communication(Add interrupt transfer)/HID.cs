using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading; //CancellationTokenSource type
using System.Threading.Tasks;

using Microsoft.Win32.SafeHandles;
using System.Diagnostics;// Debug
using System.IO;//FileStream type
using System.Runtime.InteropServices;//Marshal
using System.Windows.Forms;

namespace USB_HID_Communication
{
    internal sealed partial class HID
    {
        private const String ModuleName = "HID";
        internal Native_Methods.HIDP_CAPS Capabilities;
        internal Native_Methods.HIDD_ATTRIBUTES DeviceAttributes;//VendorID, ProductID, VersionNumber

        /// <summary>
        ///     Remove any Input reports waiting in the buffer.
        /// </summary>
        ///  
        /// <returns> True on success, False on failure. </returns>
        /*
            API function:   HidD_FlushQueue
            Purpose:    Removes any Input reports waiting in the buffer.
            Accepts:    a handle to the device.
            Returns:    True on success, False on failure.
        */       
        internal Boolean FlushQueue(SafeFileHandle hidHandle)
        {
            try
            {
                Boolean success = Native_Methods.HidD_FlushQueue(hidHandle);
                return success;
            }
            catch (Exception ex)
            {
                DisplayException(ModuleName, ex);
                throw;
            }
        }


        /// <summary>
        ///     Get HID attributes.
        /// </summary>
        /// 
        /// <returns> true on success </returns>
        /// 
        /*
            API function: HidD_GetAttributes
            Purpose:    Retrieves a HIDD_ATTRIBUTES structure containing the Vendor ID, 
                        Product ID, and Product Version Number for a device.
            Accepts:
                        A handle returned by CreateFile.
                        A pointer to receive a HIDD_ATTRIBUTES structure.
            Returns:    True on success, False on failure.
        */   
        internal Boolean GetAttributes(SafeFileHandle hidHandle, ref Native_Methods.HIDD_ATTRIBUTES deviceAttributes)
        {
            Boolean success;
            try
            {
                success = Native_Methods.HidD_GetAttributes(hidHandle, ref deviceAttributes);
            }
            catch (Exception ex)
            {
                DisplayException(ModuleName, ex);
                throw;
            }
            return success;
        }


        ///  <summary>
        ///  Retrieves a structure with information about a device's capabilities. 
        ///  </summary>
        /// 
        ///  <returns> An HIDP_CAPS structure. </returns>
        /*
            API function:   HidD_GetPreparsedData
            Purpose:    Retrieves a pointer to a buffer containing information about the device's capabilities.
                        HidP_GetCaps and other API functions require a pointer to the buffer.
            Requires: 
                        A handle returned by CreateFile.
                        A pointer to a buffer.
            Returns:    True on success, False on failure.
        */
        ///
        /*
            API function:   HidP_GetCaps
            Purpose:    Find out a device's capabilities.
                        For standard devices such as joysticks, you can find out the specific capabilities of the device.
                        For a custom device where the software knows what the device is capable of,this call may be unneeded.
            Accepts:    
                        A pointer returned by HidD_GetPreparsedData 
                        A pointer to a HIDP_CAPS structure.
            Returns:    True on success, False on failure.             
         */
        //
        /*
            API function:    HidP_GetValueCaps
            Purpose:   Retrieves a buffer containing an array of HidP_ValueCaps structures.
                       Each structure defines the capabilities of one value.
                       This application doesn't use this data.
            Accepts:
                        A report type enumerator from hidpi.h,
                        A pointer to a buffer for the returned array,
                        The NumberInputValueCaps member of the device's HidP_Caps structure,
                        A pointer to the PreparsedData structure returned by HidD_GetPreparsedData.
            Returns:    True on success, False on failure.
         */
        //
        /*
            API function:   HidD_FreePreparsedData
            Purpose:        Frees the buffer reserved by HidD_GetPreparsedData.
            Accepts:        A pointer to the PreparsedData structure returned by HidD_GetPreparsedData.
            Returns:        True on success, False on failure.
         */
        internal Native_Methods.HIDP_CAPS GetDeviceCapabilities(SafeFileHandle hidHandle)
        {
            var preparsedData = new IntPtr();

            try
            {
                Native_Methods.HidD_GetPreparsedData(hidHandle, ref preparsedData);

                 Int32 result = Native_Methods.HidP_GetCaps(preparsedData, ref Capabilities);
                if ((result != 0))
                {
                    Debug.WriteLine("");
                    Debug.WriteLine("  Usage: " + Convert.ToString(Capabilities.Usage, 16));
                    Debug.WriteLine("  Usage Page: " + Convert.ToString(Capabilities.UsagePage, 16));
                    Debug.WriteLine("  Input Report Byte Length: " + Capabilities.InputReportByteLength);
                    Debug.WriteLine("  Output Report Byte Length: " + Capabilities.OutputReportByteLength);
                    Debug.WriteLine("  Feature Report Byte Length: " + Capabilities.FeatureReportByteLength);
                    Debug.WriteLine("  Number of Link Collection Nodes: " + Capabilities.NumberLinkCollectionNodes);
                    Debug.WriteLine("  Number of Input Button Caps: " + Capabilities.NumberInputButtonCaps);
                    Debug.WriteLine("  Number of Input Value Caps: " + Capabilities.NumberInputValueCaps);
                    Debug.WriteLine("  Number of Input Data Indices: " + Capabilities.NumberInputDataIndices);
                    Debug.WriteLine("  Number of Output Button Caps: " + Capabilities.NumberOutputButtonCaps);
                    Debug.WriteLine("  Number of Output Value Caps: " + Capabilities.NumberOutputValueCaps);
                    Debug.WriteLine("  Number of Output Data Indices: " + Capabilities.NumberOutputDataIndices);
                    Debug.WriteLine("  Number of Feature Button Caps: " + Capabilities.NumberFeatureButtonCaps);
                    Debug.WriteLine("  Number of Feature Value Caps: " + Capabilities.NumberFeatureValueCaps);
                    Debug.WriteLine("  Number of Feature Data Indices: " + Capabilities.NumberFeatureDataIndices);
                    Int32 vcSize = Capabilities.NumberInputValueCaps;
                    var valueCaps = new Byte[vcSize];

                    Native_Methods.HidP_GetValueCaps(Native_Methods.HidP_Input, valueCaps, ref vcSize, preparsedData);
                    // (To use this data, copy the ValueCaps byte array into an array of structures.)              
                }
            }
            catch (Exception ex)
            {
                DisplayException(ModuleName, ex);
                throw;
            }
            finally
            {
                if (preparsedData != IntPtr.Zero)
                {
                    Native_Methods.HidD_FreePreparsedData(preparsedData);
                }
            }
            return Capabilities;
        }// Native_Methods.HIDP_CAPS

        ///  <summary>
        ///  reads a Feature report from the device.
        ///  </summary>
        ///  
        /*
            API function:   HidD_GetFeature
                            Attempts to read a Feature report from the device.
            Requires:
                            A handle to a HID
                            A pointer to a buffer containing the report ID and report
                            The size of the buffer. 
            Returns:        true on success, false on failure.
        */     
        internal Boolean GetFeatureReport(SafeFileHandle hidHandle, ref Byte[] inFeatureReportBuffer)
        {
            try
            {
                Boolean success = false;

                if (!hidHandle.IsInvalid && !hidHandle.IsClosed)
                {
                    success = Native_Methods.HidD_GetFeature(hidHandle, inFeatureReportBuffer, inFeatureReportBuffer.Length);
                    Debug.Print("HidD_GetFeature success = " + success);
                }
                return success;
            }
            catch (Exception ex)
            {
                DisplayException(ModuleName, ex);
                throw;
            }
        }

        ///  <summary>
        ///  Writes a Feature report to the device.
        ///  </summary>
        ///  
        ///  <returns>
        ///   True on success. False on failure.
        ///  </returns> 
        ///                  
        /*
            API function:   HidD_SetFeature
            Purpose:        Attempts to send a Feature report to the device.
            Accepts:
                            A handle to a HID
                            A pointer to a buffer containing the report ID and report
                            The size of the buffer. 
            Returns:        true on success, false on failure.
        */
        internal Boolean SendFeatureReport(SafeFileHandle hidHandle, Byte[] outFeatureReportBuffer)
        {
            try
            {
                Boolean success = Native_Methods.HidD_SetFeature(hidHandle, outFeatureReportBuffer, outFeatureReportBuffer.Length);
                Debug.Print("HidD_SetFeature success = " + success);
                return success;
            }
            catch (Exception ex)
            {
                DisplayException(ModuleName, ex);
                throw;
            }
        }

        /// <summary>
        /// Get the HID-class GUID
        /// </summary>
        ///
        /*
            API function:   HidD_GetHidGuid
            Purpose:        Retrieves the interface class GUID for the HID class.
            Accepts:        A System.Guid object for storing the GUID.
        */
        internal Guid GetHidGuid()
        {
            Guid hidGuid = Guid.Empty;
            try
            {
                Native_Methods.HidD_GetHidGuid(ref hidGuid);
            }
            catch (Exception ex)
            {
                DisplayException(ModuleName, ex);
                throw;
            }
            return hidGuid;
        }

        ///  <summary>
		///  Creates a 32-bit Usage from the Usage Page and Usage ID. 
		///  Determines whether the Usage is a system mouse or keyboard.
		///  Can be modified to detect other Usages.
		///  </summary>
		///  
		///  <returns>
		///  A String describing the Usage.
		///  </returns>
		internal String GetHidUsage(Native_Methods.HIDP_CAPS myCapabilities)
		{
			String usageDescription = "";
			try
			{
				//  Create32-bit Usage from Usage Page and Usage ID.
				Int32 usage = myCapabilities.UsagePage * 256 + myCapabilities.Usage;

				if (usage == Convert.ToInt32(0X102))
				{
					usageDescription = "mouse";
				}
				if (usage == Convert.ToInt32(0X106))
				{
					usageDescription = "keyboard";
				}
			}
			catch (Exception ex)
			{
				DisplayException(ModuleName, ex);
				throw;
			}
			return usageDescription;
		}

        ///  <summary>
        ///  reads an Input report from the device using a control transfer.
        ///  </summary>  
        ///  
        /*
            API function:   HidD_GetInputReport
            Purpose:        Attempts to read an Input report from the device using a control transfer.
            Requires:
                            A handle to a HID
                            A pointer to a buffer containing the report ID and report
                            The size of the buffer. 
            Returns:        true on success, false on failure.
        */
        internal Boolean GetInputReportViaControlTransfer(SafeFileHandle hidHandle, ref Byte[] inputReportBuffer)
        {
            var success = false;
            try
            {
                if (!hidHandle.IsInvalid && !hidHandle.IsClosed)
                {
                    success = Native_Methods.HidD_GetInputReport(hidHandle, inputReportBuffer, inputReportBuffer.Length + 1);
                    Debug.Print("HidD_GetInputReport success = " + success);
                }
                return success;
            }

            catch (Exception ex)
            {
                DisplayException(ModuleName, ex);
                throw;
            }
        }

        ///  <summary>
        ///  Writes an Output report to the device using a control transfer.
        ///  </summary>
        ///  
        ///  <returns>
        ///   True on success. False on failure.
        ///  </returns>
        ///  
        /*
            API function:   HidD_SetOutputReport
            Purpose:        Attempts to send an Output report to the device using a control transfer.
            Accepts:
                            A handle to a HID
                            A pointer to a buffer containing the report ID and report
                            The size of the buffer. 
            Returns:        true on success, false on failure.
        */  
        internal Boolean SendOutputReportViaControlTransfer(SafeFileHandle hidHandle, Byte[] outputReportBuffer)
        {
            try
            {                  
                Boolean success = Native_Methods.HidD_SetOutputReport(hidHandle, outputReportBuffer, outputReportBuffer.Length + 1);               
                Debug.Print("HidD_SetOutputReport success = " + success);
                return success;
            }
            catch (Exception ex)
            {
                DisplayException(ModuleName, ex);
                throw;
            }
        }

        ///  <summary>
        ///  Reads an Input report from the device using an interrupt transfer.
        ///  </summary>
        ///  
        /// <returns>
        ///   True on success. False on failure.
        ///  </returns>  
        internal async Task<Int32> GetInputReportViaInterruptTransfer(FileStream deviceData, Byte[] inputReportBuffer, CancellationTokenSource cts)
        {
            try
            {
                Int32 bytesRead = 0;

                // Begin reading an Input report. 
                Task<Int32> t = deviceData.ReadAsync(inputReportBuffer, 0, inputReportBuffer.Length, cts.Token);

                bytesRead = await t;

                // Gets to here only if the read operation completed before a timeout.
                Debug.Print("Asynchronous read completed. Bytes read = " + Convert.ToString(bytesRead));

                switch (t.Status)// The operation has one of these completion states:
                {
                    case TaskStatus.RanToCompletion:
                        Debug.Print("Input report received from device");
                        break;
                    case TaskStatus.Canceled:
                        Debug.Print("Task canceled");
                        break;
                    case TaskStatus.Faulted:
                        Debug.Print("Unhandled exception");
                        break;
                }
                return bytesRead;
            }
            catch (Exception ex)
            {
                DisplayException(ModuleName, ex);
                throw;
            }
        }

        ///  <summary>
        ///  Writes an Output report to the device using an interrupt transfer.
        ///  </summary>
        ///    
        ///  <returns>
        ///   1 on success. 0 on failure.
        ///  </returns>            
        internal async Task<Boolean> SendOutputReportViaInterruptTransfer(FileStream deviceData, SafeFileHandle hidHandle, Byte[] outputReportBuffer, CancellationTokenSource cts)
        {
            try
            {
                var success = false;

                // Begin writing the Output report. 
                Task t = deviceData.WriteAsync(outputReportBuffer, 0, outputReportBuffer.Length, cts.Token);

                await t;

                // Gets to here only if the write operation completed before a timeout.
                Debug.Print("Asynchronous write completed");

                // The operation has one of these completion states:
                switch (t.Status)
                {
                    case TaskStatus.RanToCompletion:
                        success = true;
                        Debug.Print("Output report written to device");
                        break;
                    case TaskStatus.Canceled:
                        Debug.Print("Task canceled");
                        break;
                    case TaskStatus.Faulted:
                        Debug.Print("Unhandled exception");
                        break;
                }
                return success;
            }
            catch (Exception ex)
            {
                DisplayException(ModuleName, ex);
                throw;
            }
        }

        ///  <summary>
        ///  Writes an Output report to the device using an interrupt transfer but not using async.
        ///  </summary>
        ///  
        ///  <param name="fileStreamDeviceData"> the Filestream for writing data. </param> 
        ///  <param name="hidHandle"> SafeFileHandle to the device.  </param>
        ///  <param name="outputReportBuffer"> contains the report ID and report data. </param>
        ///            
        internal void SendOutputReportViaInterruptTransfer2(FileStream fileStreamDeviceData, SafeFileHandle hidHandle, Byte[] outputReportBuffer)
        {
            try
            {
                // Begin writing the Output report. 
                fileStreamDeviceData.Write(outputReportBuffer, 0, outputReportBuffer.Length);
                fileStreamDeviceData.Flush();
            }
            catch (Exception ex)
            {
                DisplayException(ModuleName + "SendOutputReportViaInterruptTransfer2", ex);
                //throw;
            }
        }
        /// <summary>
        /// Attempts to open a handle to a HID.
        /// </summary>
        /// <returns> hidHandle - a handle to the HID </returns>
        ///
        /*
            API function:   CreateFile
            Purpose:        Retrieves a handle to a device.
            Accepts:
                            A device path name returned by SetupDiGetDeviceInterfaceDetail 
                            The type of access requested (read/write).
                            FILE_SHARE attributes to allow other processes to access the device while this handle is open.
                            A Security structure or IntPtr.Zero. 
                            A creation disposition value. Use OPEN_EXISTING for devices.
                            Flags and attributes for files. Not used for devices.
                            Handle to a template file. Not used.
            Returns:        
                            A handle without read or write access.
                            This enables obtaining information about all HIDs, even system keyboards and mice. 
                            Separate handles are used for reading and writing.
        */
        internal SafeFileHandle OpenHandle(String devicePathName, Boolean readAndWrite)
        {
            SafeFileHandle hidHandle;
            try
            {
                if (readAndWrite)
                {
                    hidHandle = FileIO_Declarations.CreateFile(devicePathName, FileIO_Declarations.GenericRead | FileIO_Declarations.GenericWrite, FileIO_Declarations.FileShareRead | FileIO_Declarations.FileShareWrite, IntPtr.Zero, FileIO_Declarations.OpenExisting, 0, IntPtr.Zero);
                }
                else
                {
                    hidHandle = FileIO_Declarations.CreateFile(devicePathName, 0, FileIO_Declarations.FileShareRead | FileIO_Declarations.FileShareWrite, IntPtr.Zero, FileIO_Declarations.OpenExisting, 0, IntPtr.Zero);
                }
            }
            catch (Exception ex)
            {
                DisplayException(ModuleName, ex);
                throw;
            }
            return hidHandle;
        }

        ///  <summary>
        ///  Provides a central mechanism for exception handling.
        ///  Displays a message box that describes the exception.
        ///  </summary>
        ///  
        internal static void DisplayException(String moduleName, Exception e)
        {
            //  Create an error message.
            String message = "Exception: " + e.Message + Environment.NewLine + "Module: " + moduleName + Environment.NewLine + "Method: " + e.TargetSite.Name;
            const String caption = "Unexpected Exception";
            MessageBox.Show(message, caption, MessageBoxButtons.OK);
            Debug.Write(message);

            // Get the last error and display it. 
            Int32 error = Marshal.GetLastWin32Error();
            Debug.WriteLine("The last Win32 Error was: " + error);
        }
    }//class HID
}
