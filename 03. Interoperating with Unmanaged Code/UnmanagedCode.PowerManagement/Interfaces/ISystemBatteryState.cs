using System.Runtime.InteropServices;

namespace UnmanagedCode.PowerManagement.Interfaces
{
    [ComVisible(true)]
    [Guid("f1a82116-cfbb-4012-abdc-6c3a2862cff3")]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface ISystemBatteryState
    {
        bool AcOnLine { get; set; }

        bool BatteryPresent { get; set; }

        bool Charging { get; set; }

        bool Discharging { get; set; }

        uint MaxCapacity { get; set; }

        uint RemainingCapacity { get; set; }

        int Rate { get; set; }

        uint EstimatedTime { get; set; }
    }
}
