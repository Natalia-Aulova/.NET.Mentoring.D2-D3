using System.Runtime.InteropServices;

namespace UnmanagedCode.PowerManagement.InternalModels
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct SystemBatteryStateInternal
    {
        [MarshalAs(UnmanagedType.I1)]
        public bool AcOnLine;

        [MarshalAs(UnmanagedType.I1)]
        public bool BatteryPresent;

        [MarshalAs(UnmanagedType.I1)]
        public bool Charging;

        [MarshalAs(UnmanagedType.I1)]
        public bool Discharging;

        public byte Spare1;

        public byte Spare2;

        public byte Spare3;

        public byte Spare4;

        public uint MaxCapacity;

        public uint RemainingCapacity;

        public int Rate;

        public uint EstimatedTime;

        public uint DefaultAlert1;

        public uint DefaultAlert2;
    }
}
