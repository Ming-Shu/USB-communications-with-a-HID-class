# USB-communications-with-a-HID-class
Purpose: 

Demonstrates USB communications with a generic HID-class device(control Output and interrupt Input only in code)

Requirements:

This software was written using Visual Studio 2013 for Windows desktop building for the .NET Framework v4.5.
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

User Interface:

![image](https://github.com/Ming-Shu/USB-communications-with-a-HID-class/blob/master/Interface_Explain.PNG)
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

This project includes the following modules:

Form1.cs                        - routines specific to the form.

HidAction.cs                    - routines specific to HID of interface module.

Hid.cs                          - routines specific to HID communications.

HidDeclarations.cs              - Declarations for API functions used by Hid.cs.

DeviceManagement.cs             - routine for obtaining a handle to a device from its GUID.

DeviceManagementDeclarations.cs - Declarations for API functions used by DeviceManagement.cs.

FileIODeclarations.cs           - Declarations for file-related API functions.

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

HID Concepts

    HID is built on a couple of fundamental concepts, a Report Descriptor, and reports.
  
    Reports:            The actual data blobs that are exchanged between a device and a software client. 
    Report Descriptor:  The format and meaning of each data blob that it supports.
    
    
 Reports
 
    ReportsThere are three report types: Input Reports, Output Reports, and Feature Reports.
    
    1.Input Report  :Data blobs that are sent from the HID device to the application, typically when the state of a control changes.
    2.Output Report :Data blobs that are sent from the application to the HID device
    3.Feature Report:Data blobs that can be manually read and/or written, and are typically related to configuration information.
    
 
 Reference:http://janaxelson.com/hidpage.htm 
