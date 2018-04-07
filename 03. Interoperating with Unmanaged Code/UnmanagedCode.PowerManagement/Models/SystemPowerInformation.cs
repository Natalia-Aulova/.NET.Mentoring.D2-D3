using System.Runtime.InteropServices;
using UnmanagedCode.PowerManagement.Interfaces;

namespace UnmanagedCode.PowerManagement.Models
{
    [ComVisible(true)]
    [Guid("3aa821ed-c63d-4eb3-b3f8-19ea38becb21")]
    [ClassInterface(ClassInterfaceType.None)]
    public class SystemPowerInformation : ISystemPowerInformation
    {
        public uint MaxIdlenessAllowed { get; set; }

        public uint Idleness { get; set; }

        public uint TimeRemaining { get; set; }

        public byte CoolingMode { get; set; }
    }
}
