# USB-communications-with-a-HID-class
Purpose: 
Demonstrates USB communications with a generic HID-class device(control Output and interrupt Input only in code)

Requirements:
This software was written using Visual Studio 2013 for Windows desktop building for the .NET Framework v4.5.

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

This project includes the following modules:

Form1.cs - routines specific to the form.
HidAction.cs - routines specific to HID of interface module.
Hid.cs - routines specific to HID communications.
HidDeclarations.cs - Declarations for API functions used by Hid.cs.
DeviceManagement.cs - routine for obtaining a handle to a device from its GUID.
DeviceManagementDeclarations.cs - Declarations for API functions used by DeviceManagement.cs.
FileIODeclarations.cs - Declarations for file-related API functions.
