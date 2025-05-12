using System.Runtime.InteropServices;

namespace WindowsNativeRawInputWrapper.PlatformTypes
{
#pragma warning disable IDE1006 // Naming Styles
    [StructLayout(LayoutKind.Sequential)]
    internal struct RAWINPUTHEADER
    {
        /// <summary>
        /// The type of raw input. See <seealso cref="Enumerations.RawInputDeviceType"/> for possible values.
        /// </summary>
        public uint Type;
        /// <summary>
        /// The size, in bytes, of the entire input packet of data. This includes <see cref="RAWINPUT"/> plus possible extra input reports in the RAWHID variable length array.
        /// </summary>
        public uint Size;
        /// <summary>
        /// A handle to the device generating the raw input data.
        /// </summary>
        public nint DeviceHandle;
        /// <summary>
        /// The value passed in the wParam parameter of the WM_INPUT message.
        /// </summary>
        public nint WParam;
    }
#pragma warning restore IDE1006 // Naming Styles
}
