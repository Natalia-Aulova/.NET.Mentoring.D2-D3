using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnmanagedCode.PowerManagement.Tests
{
    [TestClass]
    public class PowerManagementServiceTests
    {
        [TestMethod]
        public void Test_LastSleepTime()
        {
            var service = new PowerManagementService();
            var lastSleepTime = service.GetLastSleepTime();

            Console.WriteLine($"Last sleep time: {lastSleepTime}");
        }

        [TestMethod]
        public void Test_LastWakeTime()
        {
            var service = new PowerManagementService();
            var lastWakeTime = service.GetLastWakeTime();

            Console.WriteLine($"Last wake time: {lastWakeTime}");
        }

        [TestMethod]
        public void Test_SystemBatteryState()
        {
            var service = new PowerManagementService();
            var batteryState = service.GetSystemBatteryState();

            Console.WriteLine($"Ac On Line: {batteryState.AcOnLine}");
            Console.WriteLine($"Battery Present: {batteryState.BatteryPresent}");
            Console.WriteLine($"Is Charging: {batteryState.Charging}");
            Console.WriteLine($"Is Discharging: {batteryState.Discharging}");
            Console.WriteLine($"Remaining Capacity: {batteryState.RemainingCapacity}");
            Console.WriteLine($"Max Capacity: {batteryState.MaxCapacity}");
            Console.WriteLine($"Charge percentage: {batteryState.RemainingCapacity * 100 / batteryState.MaxCapacity} %");
            Console.WriteLine($"Estimated Time: {batteryState.EstimatedTime / 60} minutes");
        }

        [TestMethod]
        public void Test_SystemPowerInformation()
        {
            var service = new PowerManagementService();
            var powerInfo = service.GetSystemPowerInformation();

            Console.WriteLine($"Max Idleness Allowed: {powerInfo.MaxIdlenessAllowed}");
            Console.WriteLine($"Idleness: {powerInfo.Idleness}");
            Console.WriteLine($"Time Remaining: {powerInfo.TimeRemaining}");
            Console.WriteLine($"Cooling Mode: {powerInfo.CoolingMode}");
        }
    }
}
