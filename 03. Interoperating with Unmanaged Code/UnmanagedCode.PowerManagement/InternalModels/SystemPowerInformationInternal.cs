using System.Runtime.InteropServices;

namespace UnmanagedCode.PowerManagement.InternalModels
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct SystemPowerInformationInternal
    {
        public uint MaxIdlenessAllowed;

        public uint Idleness;

        public uint TimeRemaining;

        public byte CoolingMode;
    }
}
