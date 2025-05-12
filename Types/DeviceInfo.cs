using System;
using System.Collections.Generic;
using WindowsNativeRawInputWrapper.PlatformTypes;

namespace WindowsNativeRawInputWrapper.Types
{
    public abstract record DeviceInfoBase(InputDevice Device, string DeviceDescription);

    public record KeyboardDeviceInfo : DeviceInfoBase
    {
        public enum Type
        {
            None,
            Enhanced101Or102KeyOrCompatible,
            Japanese,
            Korean,
            UnknowOrHid
        }

        public Type KeyboardType { get; init; }

        public uint VendorSubType { get; init; }

        public uint ScanCodeMode { get; init; }

        public uint NumberOfFunctionKeys { get; init; }

        public uint NumberOfIndicators { get; init; }

        public uint NumberOfKeysTotal { get; init; }

        public KeyboardDeviceInfo(InputDevice device, string deviceDescription, uint type) : base(device, deviceDescription)
        {
            if (Enum.IsDefined(typeof(PlatformEnumerations.KeyboardType), type))
            {
                KeyboardType = ((PlatformEnumerations.KeyboardType)type).ToInternalKeyboardType();
            }
            else
            {
                KeyboardType = Type.None;
            }
        }
    }

    public record MouseDeviceInfo : DeviceInfoBase
    {
        [Flags]
        public enum IdentificationFlag
        {
            HidMouse,
            HidWheelMouse,
            HasHorizontalWheel,
        }

        public List<IdentificationFlag> DeviceIdentification { get; init; }

        public uint NumberOfButtons { get; init; }

        public uint SampleRate { get; init; }

        public bool HasHorizontalWheel { get; init; }

        internal MouseDeviceInfo(InputDevice device, string deviceDescription, uint rawIdentificationBitfield) : base(device, deviceDescription)
        {
            DeviceIdentification = [];
            foreach (var flag in Enum.GetValues<PlatformEnumerations.MouseDeviceIdentificationFlag>())
            {
                if ((rawIdentificationBitfield & (uint)flag) == (uint)flag)
                {
                    DeviceIdentification.Add(flag.ToInternalIdentificationFlag());
                }
            }
        }
    }

    public record HidDeviceInfo : DeviceInfoBase
    {
        public uint VendorId { get; init; }

        public uint ProductId { get; init; }

        public uint VersionNumber { get; init; }

        public ushort UsagePage { get; init; }

        public ushort Usage { get; init; }

        public HidDeviceInfo(InputDevice device, string deviceDescription) : base(device, deviceDescription)
        { }
    }
}
