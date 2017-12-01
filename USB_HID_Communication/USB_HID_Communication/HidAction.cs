using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Win32.SafeHandles;
using System.IO;//FileStream type
using System.Runtime.InteropServices;//Marshal
using System.Globalization;//NumberStyles
using System.Diagnostics;// Debug
using System.Windows.Forms;
using System.Threading;//CancellationTokenSource
using System.Timers;//ElapsedEventArgs
namespace USB_HID_Communication
{
    public partial class Form1  
    {
        /// <summary>
        /// Create FindMyHid need variable
        /// </summary>
        private FileStream _deviceData;
        private String _hidUsage;
        private SafeFileHandle _hidHandle;
        private Boolean _deviceHandleObtained;//if specific device is exist!
        private HID _myHid = new HID();
        private readonly DeviceManagement _myDeviceManagement = new DeviceManagement();
        private static System.Timers.Timer _periodicSend;
        private Int32 _myProductId;
        private Int32 _myVendorId;
        Boolean timerFlag = false;
        private enum FormTypes
        {
            ListBoxStatus,
            ListBoxResult
        }

        private void periodicTimer()
        {
            const Int32 Interval = 5000;
            _periodicSend = new System.Timers.Timer(Interval);
            _periodicSend.Elapsed += PeriodicWork;
            _periodicSend.Stop();
            _periodicSend.SynchronizingObject = this;
        }
        ///  <summary>
        ///  Do periodic work.
        ///  </summary>
        ///  <remarks>
        ///  The timer is enabled only if continuous (periodic) transfers have been requested.
        ///  </remarks>		  
        private void PeriodicWork(object source, ElapsedEventArgs e)
        {
            try
            {
                ReceiveInputReport();
            }
            catch (Exception ex)
            {
                DisplayException(Name, ex);
                throw;
            }
        }

        /// <summary>
        /// Call HID functions that use Win32 API functions to locate a HID-class device by its Vendor ID and Product ID. Open a handle to the device.
        /// </summary>
        /// 
        ///  <returns>
        ///   True if the device is detected, False if not detected.
        ///  </returns>
        ///  
        private Boolean FindMyHid()
        {
            var devicePathName = new String[128];
            String myDevicePathName = "";

            if (txtVendorID.Text == "" || txtProductID.Text == "")
            {
                txtVendorID.Text = "0";
                txtProductID.Text = "0";
            }
            _myVendorId = Int32.Parse(txtVendorID.Text, NumberStyles.AllowHexSpecifier);
            _myProductId = Int32.Parse(txtProductID.Text, NumberStyles.AllowHexSpecifier);

            try
            {
                _deviceHandleObtained = false;
                CloseCommunications();

                // Get the HID-class GUID.
                Guid hidGuid = _myHid.GetHidGuid();

                //  Fill an array with the device path names of all attached HIDs.
                Boolean availableHids = _myDeviceManagement.FindDeviceFromGuid(hidGuid, ref devicePathName);
                if (availableHids)
                {
                    Int32 memberIndex = 0;

                    do
                    {
                        // Open the handle without read/write access to enable getting information about any HID, even system keyboards and mice.
                        _hidHandle = _myHid.OpenHandle(devicePathName[memberIndex], false);

                        if (!_hidHandle.IsInvalid)
                        {
                            // The returned handle is valid,so find out if this is the device we're looking for.
                            _myHid.DeviceAttributes.Size = Marshal.SizeOf(_myHid.DeviceAttributes);

                            Boolean success = _myHid.GetAttributes(_hidHandle, ref _myHid.DeviceAttributes);
                            if (success)
                            {
                                if ((_myHid.DeviceAttributes.VendorID == _myVendorId) && (_myHid.DeviceAttributes.ProductID == _myProductId))
                                {
                                    //  Display the information in form's list box.
                                    ShowInfoToForm(FormTypes.ListBoxStatus,"Handle obtained to my device:");
                                    ShowInfoToForm(FormTypes.ListBoxStatus, "Vendor ID= " + Convert.ToString(_myHid.DeviceAttributes.VendorID, 16));
                                    ShowInfoToForm(FormTypes.ListBoxStatus, "Product ID = " + Convert.ToString(_myHid.DeviceAttributes.ProductID, 16));
                                    ShowInfoToForm(FormTypes.ListBoxStatus, " ");
                                    _deviceHandleObtained = true;
                                    myDevicePathName = devicePathName[memberIndex];
                                }
                                else
                                {
                                    //  It's not a match, so close the handle.
                                    _deviceHandleObtained = false;
                                    _hidHandle.Close();
                                }
                            }
                            else
                            {
                                //  There was a problem retrieving the information.
                                Debug.WriteLine("  Error in filling HIDD_ATTRIBUTES structure.");
                                _deviceHandleObtained = false;
                                _hidHandle.Close();
                            }
                        }
                        //  Keep looking until we find the device or there are no devices left to examine.
                        memberIndex = memberIndex + 1;
                    }
                    while (!((_deviceHandleObtained || (memberIndex == devicePathName.Length))));
                }//(availableHids)
                if (_deviceHandleObtained)
                {
                    //  The device was detected.
                    //  Learn the capabilities of the device.
                    _myHid.Capabilities = _myHid.GetDeviceCapabilities(_hidHandle);

                    //  Find out if the device is a system mouse or keyboard.
                    _hidUsage = _myHid.GetHidUsage(_myHid.Capabilities);


                    //Close the handle and reopen it with read/write access.
                    _hidHandle.Close();

                    _hidHandle = _myHid.OpenHandle(myDevicePathName, true);

                    if (_hidHandle.IsInvalid)
                    {
                        ShowInfoToForm(FormTypes.ListBoxStatus, "The device is a system " + _hidUsage + ".   ");
                        ShowInfoToForm(FormTypes.ListBoxStatus, "Windows 2000 and later obtain exclusive access to Input and Output reports for this devices.");
                        ShowInfoToForm(FormTypes.ListBoxStatus, "Windows 8 also obtains exclusive access to Feature reports.");
                        ShowInfoToForm(FormTypes.ListBoxStatus, " ");
                    }
                    else
                    {
                        if (_myHid.Capabilities.InputReportByteLength > 0)
                        {
                            //  Set the size of the Input report buffer. 
                            var inputReportBuffer = new Byte[_myHid.Capabilities.InputReportByteLength];

                            _deviceData = new FileStream(_hidHandle, FileAccess.Read | FileAccess.Write, inputReportBuffer.Length, false);
                        }

                        //  Flush any waiting reports in the input buffer. (optional)
                        _myHid.FlushQueue(_hidHandle);
                    }
                }
                else
                {
                    ShowInfoToForm(FormTypes.ListBoxStatus, "Device not found.");
                }
                return _deviceHandleObtained;
            }
            catch (Exception ex)
            {
                DisplayException(Name, ex);
                throw;
            }
        }//FindMyHid

        ///  <summary>
        ///  Enables accessing a form's controls from another thread 
        ///  </summary>
        ///  
        private void ShowInfoToForm(FormTypes type, String textToDisplay)
        {
            try
            {
                object[] args = {type,textToDisplay };

                //  The AccessForm routine contains the code that accesses the form.
                InfoToForm DataToFormDelegate = AccessForm;

                //  Execute AccessForm, passing the parameters in args.
                Invoke(DataToFormDelegate, args);
            }
            catch (Exception ex)
            {
                DisplayException(Name, ex);
                throw;
            }
        }

        //  This delegate has the same parameters as AccessForm.
        //  Used in accessing the application's form from a different thread.
        private delegate void InfoToForm(FormTypes type, String textToAdd);

        ///  <summary>
        ///  Involve accessing the application's form.
        ///  </summary>
        ///  
        private void AccessForm(FormTypes type, String formText)
        {
            try
            {
                switch (type)
				{
                    case FormTypes.ListBoxStatus:
                        LstStatus.Items.Add(formText);
                        break;
                    case FormTypes.ListBoxResult:
                        LstResult.Items.Add(formText);
                        break;
            }
            }
            catch (Exception ex)
            {
                DisplayException(Name, ex);
                throw;
            }
        }

        ///  <summary>
        ///  Enables accessing a button controls from another thread 
        ///  </summary>
        ///  
        private void ReceiveButtonEnable(Boolean enable)
        {
            if (btn_receive.InvokeRequired)
            {
                object[] args = {enable};
                ButtonStatus ButtonStatus = new ButtonStatus(ReceiveButtonEnable);
                this.Invoke(ButtonStatus, args);
            }
            else
            {
                btn_receive.Enabled = enable;
            }
        }
        private delegate void ButtonStatus(Boolean enable);

        /// <summary>
        /// Close the handle and FileStreams for a device.
        /// </summary>
        /// 
        private void CloseCommunications()
        {
            if (_deviceData != null)
                _deviceData.Close();

            if ((_hidHandle != null) && (!(_hidHandle.IsInvalid)))
                _hidHandle.Close();

            // The next attempt to communicate will get a new handle and FileStreams.
            _deviceHandleObtained = false;
        }

        ///  <summary>
		///  Sends an Output report.
		///  Assumes report ID = 0.
		///  </summary>
        private void SendOutputReport(Byte[] outputBuffer,int size)
        {
            try
            {
                // If the device hasn't been detected, was removed, or timed out on a previous attempt to access it, look for the device.
                if (!_deviceHandleObtained){
                    _deviceHandleObtained = FindMyHid();
                }
                else
                {
                    if (_hidHandle.IsInvalid)
                    {
                        ShowInfoToForm(FormTypes.ListBoxStatus, "Invalid handle.");
                        ShowInfoToForm(FormTypes.ListBoxStatus, "No attempt to write an Output report or read an Input report was made.");
                    }
                    else//Don't attempt to exchange reports if valid handles aren't available (as for a mouse or keyboard.)
                    {
                        if (_myHid.Capabilities.OutputReportByteLength > 0)//  Don't attempt to send an Output report if the HID has no Output report.
                        {
                            var outputReportBuffer = new Byte[_myHid.Capabilities.OutputReportByteLength + 1];//  Set the size of the Output report buffer.  
                            outputReportBuffer[0] = 0;//  Store the report ID in the first byte of the buffer
                            Array.Copy(outputBuffer, 0, outputReportBuffer, 1, size);
                            Boolean success = _myHid.SendOutputReportViaControlTransfer(_hidHandle, outputReportBuffer);
                            //MessageBox.Show("_hidHandle: " + _hidHandle.ToString() + "success: " + success.ToString());
                        }
                    }//if (!_hidHandle.IsInvalid)
                }
            }
            catch (Exception ex)
            {
                DisplayException(Name, ex);
                throw;
            }
        }// SendOutputReport()

        ///  <summary>
        ///  Request an Input report.
        ///  Assumes report ID = 0.
        ///  </summary>
        ///  Read a report using interrupt transfers. 
        ///  Timeout if no report available.
        ///  To enable reading a report without blocking the calling thread, uses Filestream's ReadAsync method.
        ///  
        private async void ReceiveInputReport()
        {
            const Int32 readTimeout = 2000;
            Byte[] inputReportBuffer = null;
            timerFlag = true;
            try
            {
                // If the device hasn't been detected, was removed, or timed out on a previous attempt to access it, look for the device.
                if (!_deviceHandleObtained)
                {
                    _deviceHandleObtained = FindMyHid();
                }
                else
                {
                    if (_hidHandle.IsInvalid)
                    {
                        ShowInfoToForm(FormTypes.ListBoxStatus, "Invalid handle.");
                        ShowInfoToForm(FormTypes.ListBoxStatus, "No attempt to write an Output report or read an Input report was made.");
                    }
                    else//Don't attempt to exchange reports if valid handles aren't available (as for a mouse or keyboard.)
                    {
                        //The HID spec requires all HIDs to have an interrupt IN endpoint, which suggests that all HIDs must support Input reports.
                        if (_myHid.Capabilities.InputReportByteLength > 0)//  Don't attempt to send an Input report if the HID has no Input report.
                        {
                            btn_receive.Enabled = false;

                            inputReportBuffer = new Byte[_myHid.Capabilities.InputReportByteLength];

                            Action onReadTimeoutAction = OnReadTimeout;//Create a delegate to execute on a timeout.

                            var cts = new CancellationTokenSource();// The CancellationTokenSource specifies the timeout value and the action to take on a timeout.
                            cts.CancelAfter(readTimeout);// Cancel the read if it hasn't completed after a timeout.
                            cts.Token.Register(onReadTimeoutAction);// Specify the function to call on a timeout.

                            Int32 bytesRead = await _myHid.GetInputReportViaInterruptTransfer(_deviceData, inputReportBuffer, cts);
                            // Arrive here only if the operation completed.
                            cts.Dispose();// Dispose to stop the timeout timer. 
                            if (bytesRead >0)
                            {
                                ShowInfoToForm(FormTypes.ListBoxResult, " Report Data:");
                                Int32 count;
                                String byteValue = "";
                                for (count = 1; count <= inputReportBuffer.Length - 1; count++)
                                    byteValue += String.Format("{0:X2} ", inputReportBuffer[count]); //  Display bytes as 2-character Hex strings.     
                        
                                ShowInfoToForm(FormTypes.ListBoxResult, " " + byteValue);
                            }
                            else
                            {
                                CloseCommunications();
                                ShowInfoToForm(FormTypes.ListBoxResult, "The attempt to read an Input report has failed.");
                            }
                        }
                        else
                        {
                            ShowInfoToForm(FormTypes.ListBoxStatus, "No attempt to read an Input report was made");
                            ShowInfoToForm(FormTypes.ListBoxStatus, "The HID doesn't have an Input report.");
                        }
                    }//if (!_hidHandle.IsInvalid)                 
                }
                btn_receive.Enabled = true;
                timerFlag = false;
            }
            catch (Exception ex)
            {
                DisplayException(Name, ex);
                throw;
            }
        }// ReceiveInputReport()

        /// <summary>
        /// Timeout if read via interrupt transfer doesn't return.
        /// </summary>
        private void OnReadTimeout()
        {
            try
            {
                ShowInfoToForm(FormTypes.ListBoxStatus, "The attempt to read a report timed out.");
                ShowInfoToForm(FormTypes.ListBoxStatus, " ");
                ReceiveButtonEnable(true);
                CloseCommunications();
                FindMyHid();
                timerFlag = false;
            }
            catch (Exception ex)
            {
                DisplayException(Name, ex);
                throw;
            }
        }

    }//Form1
}
