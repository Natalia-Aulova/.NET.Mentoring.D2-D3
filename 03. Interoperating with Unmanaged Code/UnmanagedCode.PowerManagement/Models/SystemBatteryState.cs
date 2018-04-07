using System.Runtime.InteropServices;
using UnmanagedCode.PowerManagement.Interfaces;

namespace UnmanagedCode.PowerManagement.Models
{
    [ComVisible(true)]
    [Guid("1942b827-f3e5-480a-a1d7-b850dd612802")]
    [ClassInterface(ClassInterfaceType.None)]
    public class SystemBatteryState : ISystemBatteryState
    {
        public bool AcOnLine { get; set; }

        public bool BatteryPresent { get; set; }

        public bool Charging { get; set; }

        public bool Discharging { get; set; }

        public uint MaxCapacity { get; set; }

        public uint RemainingCapacity { get; set; }

        public int Rate { get; set; }

        public uint EstimatedTime { get; set; }
    }
}
