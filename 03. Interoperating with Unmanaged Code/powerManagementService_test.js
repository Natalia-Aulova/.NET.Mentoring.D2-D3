var powerManagementService = new ActiveXObject("UnmanagedCode.PowerManagement.PowerManagementService");

var lastSleepTime = powerManagementService.GetLastSleepTime();

WScript.Echo("Last sleep time: ");
WScript.Echo(lastSleepTime);

var lastWakeTime = powerManagementService.GetLastWakeTime();

WScript.Echo("\nLast wake time: ");
WScript.Echo(lastWakeTime);

var batteryInfo = powerManagementService.GetSystemBatteryState();

WScript.Echo("\nSystem battery state\n");
WScript.Echo("Ac on line: " + batteryInfo.AcOnLine);
WScript.Echo("Battery present: " + batteryInfo.BatteryPresent);
WScript.Echo("Charging: " + batteryInfo.Charging);
WScript.Echo("Discharging: " + batteryInfo.Discharging);
WScript.Echo("Remaining capacity: " + batteryInfo.RemainingCapacity);
WScript.Echo("Max capacity: " + batteryInfo.MaxCapacity);
WScript.Echo("Charge percentage: " + Math.round(batteryInfo.RemainingCapacity * 100 / batteryInfo.MaxCapacity) + " %");
WScript.Echo("Rate: " + batteryInfo.Rate);
WScript.Echo("Estimated time: " + Math.round(batteryInfo.EstimatedTime / 60) + " minutes\n");

var powerInfo = powerManagementService.GetSystemPowerInformation();

WScript.Echo("\nSystem power information\n");
WScript.Echo("Max idleness allowed: " + powerInfo.MaxIdlenessAllowed);
WScript.Echo("Idleness: " + powerInfo.Idleness);
WScript.Echo("Time remaining: " + powerInfo.TimeRemaining);
WScript.Echo("Cooling mode: " + powerInfo.CoolingMode);
