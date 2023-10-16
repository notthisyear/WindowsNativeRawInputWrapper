﻿using System;
using System.Runtime.InteropServices;
using WindowsNativeRawInputWrapper;

namespace WindowsNativeRawInputWrapper.PlatformTypes
{
#pragma warning disable IDE1006 // Naming Styles
    [StructLayout(LayoutKind.Sequential)]
    internal struct RAWINPUTDEVICELIST
    {
        /// <summary>
        /// A handle to the raw input device.
        /// </summary>
        public nint Device;

        /// <summary>
        /// The type of device. See <seealso cref="Enumerations.RawInputDeviceType"/> for possible values.
        /// </summary>
        public uint Type;
    }
#pragma warning restore IDE1006 // Naming Styles
}
