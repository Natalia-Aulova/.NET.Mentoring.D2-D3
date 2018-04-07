1. Create a library for Power State management based on Power Management API (https://msdn.microsoft.com/en-us/library/windows/desktop/bb968807(v=vs.85).aspx).
   The library should support the following:
	a) Get the current information (based on CallNtPowerInformation function) about power management such as:
		- LastSleepTime
		- LastWakeTime
		- SystemBatteryState
		- SystemPowerInformation
	b) Reserve and remove hibernation file (see also CallNtPowerInformation function)
	c) Put a computer into sleep/hibernation state (see SetSuspendState)

2. Based on this library create a COM component which will be accessible from script languages and VBA (with IDispatch support).

3. Write test scripts (based on VBScript/JScript) which work with the library.


Development notes:

1) To register DLL as a COM component:
	a) Go to the folder which contains DLL;
	b) Run the following command in CMD:
	   C:\Windows\Microsoft.NET\Framework64\v4.0.30319\RegAsm.exe UnmanagedCode.PowerManagement.dll /tlb /codebase

2) To unregister DLL as a COM component:
	a) Go to the folder which contains DLL;
	b) Run the following command in CMD:
	   C:\Windows\Microsoft.NET\Framework64\v4.0.30319\RegAsm.exe UnmanagedCode.PowerManagement.dll /unregister

3) To run JScript file:
	a) Go to the folder which contains JScript file;
	b) Run the following command in CMD:
	   cscript powerManagementService_test.js