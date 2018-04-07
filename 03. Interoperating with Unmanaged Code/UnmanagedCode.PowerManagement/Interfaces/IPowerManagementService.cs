using System.Runtime.InteropServices;
using UnmanagedCode.PowerManagement.Models;

namespace UnmanagedCode.PowerManagement.Interfaces
{
    [ComVisible(true)]
    [Guid("fc21d64c-8f92-4c80-aaad-f6e2d0b88212")]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IPowerManagementService
    {
        ulong GetLastSleepTime();

        ulong GetLastWakeTime();

        SystemBatteryState GetSystemBatteryState();

        SystemPowerInformation GetSystemPowerInformation();

        void Sleep();

        void Hibernate();
    }
}
