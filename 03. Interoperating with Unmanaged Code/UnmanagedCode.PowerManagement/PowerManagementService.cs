using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using UnmanagedCode.PowerManagement.Interfaces;
using UnmanagedCode.PowerManagement.InternalModels;
using UnmanagedCode.PowerManagement.Models;

namespace UnmanagedCode.PowerManagement
{
    [ComVisible(true)]
    [Guid("edbf629d-f6d5-48c1-bb21-22dba6fd369d")]
    [ClassInterface(ClassInterfaceType.None)]
    public class PowerManagementService : IPowerManagementService
    {
        private const uint SuccessStatus = 0;

        public ulong GetLastSleepTime()
        {
            return GetInformation<ulong>(InformationLevelConstants.LastSleepTime);
        }

        public ulong GetLastWakeTime()
        {
            return GetInformation<ulong>(InformationLevelConstants.LastWakeTime);
        }

        public SystemBatteryState GetSystemBatteryState()
        {
            var info = GetInformation<SystemBatteryStateInternal>(InformationLevelConstants.SystemBatteryState);

            return new SystemBatteryState
            {
                AcOnLine = info.AcOnLine,
                BatteryPresent = info.BatteryPresent,
                Charging = info.Charging,
                Discharging = info.Discharging,
                MaxCapacity = info.MaxCapacity,
                RemainingCapacity = info.RemainingCapacity,
                Rate = info.Rate,
                EstimatedTime = info.EstimatedTime
            };
        }

        public SystemPowerInformation GetSystemPowerInformation()
        {
            var info = GetInformation<SystemPowerInformationInternal>(InformationLevelConstants.SystemPowerInformation);

            return new SystemPowerInformation
            {
                MaxIdlenessAllowed = info.MaxIdlenessAllowed,
                Idleness = info.Idleness,
                TimeRemaining = info.TimeRemaining,
                CoolingMode = info.CoolingMode
            };
        }

        public void Sleep()
        {
            SetSuspendState(false, false, false);
        }

        public void Hibernate()
        {
            SetSuspendState(true, false, false);
        }

        public bool ReserveHiberFile(bool reserveHiberFile)
        {
            var bufferSize = Marshal.SizeOf(typeof(bool));
            var buffer = IntPtr.Zero;

            try
            {
                buffer = Marshal.AllocCoTaskMem(bufferSize);
                Marshal.StructureToPtr(reserveHiberFile, buffer, true);
                var retval = CallNtPowerInformation(InformationLevelConstants.SystemReserveHiberFile, buffer, bufferSize, IntPtr.Zero, 0);

                if (retval != SuccessStatus)
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                }
            }
            finally
            {
                Marshal.FreeCoTaskMem(buffer);
            }

            var powerCapabilities = GetInformation<SystemPowerCapabilitiesInternal>(InformationLevelConstants.SystemPowerCapabilities);

            return powerCapabilities.HiberFilePresent == reserveHiberFile;
        }

        private T GetInformation<T>(int informationLevel)
        {
            var bufferSize = Marshal.SizeOf(typeof(T));
            var buffer = IntPtr.Zero;

            try
            {
                buffer = Marshal.AllocCoTaskMem(bufferSize);
                var retval = CallNtPowerInformation(informationLevel, IntPtr.Zero, 0, buffer, bufferSize);

                if (retval != SuccessStatus)
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                }

                return Marshal.PtrToStructure<T>(buffer);
            }
            finally
            {
                Marshal.FreeCoTaskMem(buffer);
            }
        }

        [DllImport("powrprof.dll", SetLastError = true)]
        private static extern uint CallNtPowerInformation(
            [In] int InformationLevel,
            [In] IntPtr lpInputBuffer,
            [In] int nInputBufferSize,
            [Out] IntPtr lpOutputBuffer,
            [In] int nOutputBufferSize
        );

        [DllImport("powrprof.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool SetSuspendState(
            [In, MarshalAs(UnmanagedType.I1)] bool hibernate,
            [In, MarshalAs(UnmanagedType.I1)] bool forceCritical,
            [In, MarshalAs(UnmanagedType.I1)] bool disableWakeEvent
        );
    }
}
