using System.Runtime.InteropServices;

namespace UnmanagedCode.PowerManagement.Interfaces
{
    [ComVisible(true)]
    [Guid("07393067-adad-4042-9bd2-79846d9d3041")]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface ISystemPowerInformation
    {
        uint MaxIdlenessAllowed { get; set; }

        uint Idleness { get; set; }

        uint TimeRemaining { get; set; }

        byte CoolingMode { get; set; }
    }
}
