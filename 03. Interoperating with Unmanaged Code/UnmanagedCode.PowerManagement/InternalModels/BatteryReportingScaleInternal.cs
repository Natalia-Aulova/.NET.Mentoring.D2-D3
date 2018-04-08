using System.Runtime.InteropServices;

namespace UnmanagedCode.PowerManagement.InternalModels
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct BatteryReportingScaleInternal
    {
        public ulong Granularity;

        public ulong Capacity;
    }
}
