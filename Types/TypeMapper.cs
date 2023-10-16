using System;
using static WindowsNativeRawInputWrapper.PlatformTypes.PlatformEnumerations;

namespace WindowsNativeRawInputWrapper.Types
{
    internal static class TypeMapper
    {
        public static DeviceType ToDeviceType(this RawInputDeviceType rawInputDeviceType)
            => rawInputDeviceType switch
            {
                RawInputDeviceType.RIM_TYPEMOUSE => DeviceType.Mouse,
                RawInputDeviceType.RIM_TYPEKEYBOARD => DeviceType.Keyboard,
                RawInputDeviceType.RIM_TYPEHID => DeviceType.Hid,
                _ => throw new NotSupportedException()
            };

        public static KeyboardDeviceInfo.Type ToInternalKeyboardType(this KeyboardType keyboardType)
            => keyboardType switch
            {
                KeyboardType.Enhanced101Or102KeyKeyboardOrCompatible => KeyboardDeviceInfo.Type.Enhanced101Or102KeyOrCompatible,
                KeyboardType.JapaneseKeyboard => KeyboardDeviceInfo.Type.Japanese,
                KeyboardType.KoreanKeyboard => KeyboardDeviceInfo.Type.Korean,
                KeyboardType.UnknownTypeOrHidKeyboard => KeyboardDeviceInfo.Type.UnknowOrHid,
                _ => throw new NotImplementedException(),
            };

        public static MouseDeviceInfo.IdentificationFlag ToInternalIdentificationFlag(this MouseDeviceIdentificationFlag mouseDeviceIdentificationFlag)
            => mouseDeviceIdentificationFlag switch
            {
                MouseDeviceIdentificationFlag.MOUSE_HID_HARDWARE => MouseDeviceInfo.IdentificationFlag.HidMouse,
                MouseDeviceIdentificationFlag.WHEELMOUSE_HID_HARDWARE => MouseDeviceInfo.IdentificationFlag.HidWheelMouse,
                MouseDeviceIdentificationFlag.HORIZONTAL_WHEEL_PRESENT => MouseDeviceInfo.IdentificationFlag.HasHorizontalWheel,
                _ => throw new NotImplementedException()
            };
    }
}
