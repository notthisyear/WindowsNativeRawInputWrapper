using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using Microsoft.Win32;
using WindowsNativeRawInputWrapper.PlatformTypes;
using WindowsNativeRawInputWrapper.Types;
using static WindowsNativeRawInputWrapper.PlatformTypes.PlatformEnumerations;

namespace WindowsNativeRawInputWrapper
{
    public partial class WinApiWrapper
    {
        private const uint WM_INPUT = 0x00FF;

        #region Public methods
        public static bool IsInputMessage(uint msg)
            => msg == WM_INPUT;

        public static bool TryGetAllInputDevices(out List<InputDevice> devices, out string errorMessage)
        {
            devices = new List<InputDevice>();
            uint numberOfDevices = 0;
            var deviceListSize = (uint)Marshal.SizeOf(typeof(RAWINPUTDEVICELIST));
            _ = Interops.GetRawInputDeviceList(IntPtr.Zero, ref numberOfDevices, deviceListSize);

            if (LastPInvokeWasError(out errorMessage))
                return false;

            var buffer = Marshal.AllocHGlobal((int)numberOfDevices * (int)deviceListSize);
            _ = Interops.GetRawInputDeviceList(buffer, ref numberOfDevices, deviceListSize);

            if (LastPInvokeWasError(out errorMessage))
            {
                Marshal.FreeHGlobal(buffer);
                return false;
            }

            for (var i = 0; i < numberOfDevices; i++)
            {
                var device = Marshal.PtrToStructure<RAWINPUTDEVICELIST>(IntPtr.Add(buffer, (int)(deviceListSize * i)));
                if (!Enum.IsDefined(typeof(RawInputDeviceType), device.Type))
                {
                    devices.Clear();
                    Marshal.FreeHGlobal(buffer);
                    errorMessage = $"Unknown device type {device.Type}";
                    return false;
                }
                devices.Add(new(device.Device, ((RawInputDeviceType)device.Type).ToDeviceType()));
            }
            Marshal.FreeHGlobal(buffer);
            return true;
        }

        public static bool TryRegisterForKeyboardInput(long windowHandle, out string errorMessage)
        {
            return TryRegisterInputDevice(ToPtr(windowHandle),
                UsagePageAndIdBase.GetGenericDesktopControlUsagePageAndFlag(HidGenericDesktopControls.HID_USAGE_GENERIC_KEYBOARD),
                out errorMessage,
                DeviceRegistrationModeFlag.RIDEV_NOLEGACY, DeviceRegistrationModeFlag.RIDEV_EXINPUTSINK);
        }

        [SupportedOSPlatform("windows")]
        public static bool TryGetDeviceInfoForDevice(InputDevice device, out DeviceInfoBase? deviceInformation, out string errorMessage)
        {
            deviceInformation = default;
            var deviceHandle = ToPtr(device.DeviceId);
            if (!TryGetDeviceName(deviceHandle, out var deviceName, out errorMessage))
            {
                return false;
            }
            if (!TryGetDeviceInfo(deviceHandle, out var deviceInfo, out errorMessage))
                return false;

            if (!TryGetDeviceDescriptionFromRegistry(deviceName, out var deviceDescription, out errorMessage))
                return false;

            deviceInformation = device.Type switch
            {
                DeviceType.Mouse => new MouseDeviceInfo(device, deviceDescription, deviceInfo.mouse.Id)
                {
                    NumberOfButtons = deviceInfo.mouse.NumberOfButtons,
                    SampleRate = deviceInfo.mouse.SampleRate,
                    HasHorizontalWheel = deviceInfo.mouse.HasHorizontalWheel == 0x0001
                },
                DeviceType.Keyboard => new KeyboardDeviceInfo(device, deviceDescription, deviceInfo.keyboard.Type)
                {
                    VendorSubType = deviceInfo.keyboard.SubType,
                    ScanCodeMode = deviceInfo.keyboard.KeyboardMode,
                    NumberOfFunctionKeys = deviceInfo.keyboard.NumberOfFunctionKeys,
                    NumberOfIndicators = deviceInfo.keyboard.NumberOfIndicators,
                    NumberOfKeysTotal = deviceInfo.keyboard.NumberOfKeysTotal,
                },
                DeviceType.Hid => new HidDeviceInfo(device, deviceDescription)
                {
                    VendorId = deviceInfo.hid.VendorId,
                    ProductId = deviceInfo.hid.ProductId,
                    VersionNumber = deviceInfo.hid.VersionNumber,
                    UsagePage = deviceInfo.hid.UsagePage,
                    Usage = deviceInfo.hid.Usage
                },
                _ => throw new NotImplementedException(),
            }; ;
            return true;
        }

        public static bool TryGetRawInput(long rawInputHandle, out RawInputBase? input, out string errorMessage)
        {
            input = default;
            var handle = ToPtr(rawInputHandle);
            if (!TryGetHeader(handle, out var rawHeader, out var rawHeaderSize, out errorMessage))
                return false;

            if (!Enum.IsDefined(typeof(RawInputDeviceType), rawHeader.Type))
            {
                errorMessage = $"Unknown device type {rawHeader.Type}";
                return false;
            }

            return TryGetData(handle,
                new RawInputHeader(((RawInputDeviceType)rawHeader.Type).ToDeviceType(), rawHeader.DeviceHandle),
                rawHeaderSize,
                rawHeader.Size,
                out input,
                out errorMessage);
        }

        public static bool LastPInvokeWasError(out string errorMessage)
        {
            errorMessage = string.Empty;
            var errorCode = Marshal.GetLastPInvokeError();
            if (errorCode != 0x00)
                errorMessage = $"PInvoke error {errorCode} - '{Marshal.GetLastPInvokeErrorMessage()}'";
            return !string.IsNullOrEmpty(errorMessage);
        }
        #endregion

        #region Private methods
        private static IntPtr ToPtr(long l)
            => checked((IntPtr)l);

        private static bool TryRegisterInputDevice(IntPtr windowHandle, UsagePageAndIdBase usagePageAndId, out string errorMessage, params DeviceRegistrationModeFlag[] flags)
        {
            ushort flagsValue = 0x0000;
            foreach (var flag in flags)
                flagsValue |= (ushort)flag;

            var device = new RAWINPUTDEVICE()
            {
                UsagePage = (ushort)usagePageAndId.UsagePage,
                Usage = usagePageAndId.UsageId,
                Flags = flagsValue,
                WindowHandle = windowHandle
            };

            return TryRegisterDevices(out errorMessage, device);
        }

        [SupportedOSPlatform("windows")]
        private static bool TryGetDeviceDescriptionFromRegistry(string deviceName, out string deviceDescription, out string errorMessage)
        {
            deviceDescription = string.Empty;
            errorMessage = string.Empty;

            var deviceParts = deviceName[4..].Split('#');
            var classCode = deviceParts[0];       // ACPI (Class code)
            var subClassCode = deviceParts[1];    // PNP0303 (SubClass code)
            var protocolCode = deviceParts[2];    // 3&13c0b0c5&0 (Protocol code)

            var key = Registry.LocalMachine.OpenSubKey($"System\\CurrentControlSet\\Enum\\{classCode}\\{subClassCode}\\{protocolCode}");
            if (key != default)
            {
                deviceDescription = key.GetValue("DeviceDesc")?.ToString() ?? string.Empty;
                if (!string.IsNullOrEmpty(deviceDescription) && deviceDescription.Contains(';'))
                    deviceDescription = deviceDescription[(deviceDescription.IndexOf(";") + 1)..];
                else
                    errorMessage = "DeviceDesc either missing or has unexpected format";
            }
            else
            {
                errorMessage = "Could not find matching key in registry";
            }

            return string.IsNullOrEmpty(errorMessage);
        }

        private static bool TryGetDeviceName(IntPtr deviceId, out string deviceName, out string errorMessage)
        {
            deviceName = string.Empty;
            uint size = 0;
            _ = Interops.GetRawInputDeviceInfo(deviceId, (uint)GetDeviceInfoCommand.RIDI_DEVICENAME, IntPtr.Zero, ref size);

            if (LastPInvokeWasError(out errorMessage))
                return false;

            var nameBuffer = Marshal.AllocHGlobal((int)size);
            _ = Interops.GetRawInputDeviceInfo(deviceId, (uint)GetDeviceInfoCommand.RIDI_DEVICENAME, nameBuffer, ref size);

            if (LastPInvokeWasError(out errorMessage))
            {
                Marshal.FreeHGlobal(nameBuffer);
                return false;
            }

            deviceName = Marshal.PtrToStringAnsi(nameBuffer) ?? string.Empty;
            Marshal.FreeHGlobal(nameBuffer);

            if (string.IsNullOrEmpty(deviceName))
                errorMessage = "Marshal.PtrToStringAnsi() failed";
            return string.IsNullOrEmpty(errorMessage);
        }

        private static bool TryGetDeviceInfo(IntPtr deviceId, out RID_DEVICE_INFO deviceInfo, out string errorMessage)
        {
            var deviceInfoSize = (uint)Marshal.SizeOf(typeof(RID_DEVICE_INFO));
            deviceInfo = new RID_DEVICE_INFO() { Size = deviceInfoSize };

            var deviceInfoBuffer = Marshal.AllocHGlobal((int)deviceInfoSize);
            Marshal.StructureToPtr(deviceInfo, deviceInfoBuffer, false);

            _ = Interops.GetRawInputDeviceInfo(deviceId, (uint)GetDeviceInfoCommand.RIDI_DEVICEINFO, deviceInfoBuffer, ref deviceInfoSize);
            if (LastPInvokeWasError(out errorMessage))
            {
                Marshal.FreeHGlobal(deviceInfoBuffer);
                return false;
            }

            deviceInfo = Marshal.PtrToStructure<RID_DEVICE_INFO>(deviceInfoBuffer);
            Marshal.FreeHGlobal(deviceInfoBuffer);
            return true;
        }

        private static bool TryRegisterDevices(out string errorMessage, params RAWINPUTDEVICE[] devices)
        {
            var numberOfDevices = (uint)devices.Length;
            var deviceSize = (uint)Marshal.SizeOf(typeof(RAWINPUTDEVICE));
            var devicesBuffer = Marshal.AllocHGlobal((int)(numberOfDevices * deviceSize));

            var ptr = new IntPtr(devicesBuffer);
            for (var i = 0; i < devices.Length; i++)
            {
                Marshal.StructureToPtr(devices[i], ptr, false);
                ptr += Marshal.SizeOf(typeof(RAWINPUTDEVICE));
            }

            _ = Interops.RegisterRawInputDevices(devicesBuffer, numberOfDevices, deviceSize);
            Marshal.FreeHGlobal(devicesBuffer);
            return !LastPInvokeWasError(out errorMessage);
        }

        private static bool TryGetHeader(IntPtr inputHandle, out RAWINPUTHEADER header, out uint rawHeaderSize, out string errorMessage)
        {
            rawHeaderSize = (uint)Marshal.SizeOf<RAWINPUTHEADER>();
            var headerBuffer = Marshal.AllocHGlobal((int)rawHeaderSize);

            _ = Interops.GetRawInputData(inputHandle, (uint)RawInputReadCommand.RID_HEADER, headerBuffer, ref rawHeaderSize, rawHeaderSize);
            if (LastPInvokeWasError(out errorMessage))
            {
                Marshal.FreeHGlobal(headerBuffer);
                header = default;
                return false;
            }

            header = Marshal.PtrToStructure<RAWINPUTHEADER>(headerBuffer);
            Marshal.FreeHGlobal(headerBuffer);
            return true;
        }

        private static bool TryGetData(IntPtr inputHandle, RawInputHeader header, uint rawHeaderSize, uint dataSize, out RawInputBase? rawInput, out string errorMessage)
        {
            rawInput = default;
            var dataBuffer = Marshal.AllocHGlobal((int)dataSize);
            _ = Interops.GetRawInputData(inputHandle, (uint)RawInputReadCommand.RID_INPUT, dataBuffer, ref dataSize, rawHeaderSize);
            if (LastPInvokeWasError(out errorMessage))
            {
                Marshal.FreeHGlobal(dataBuffer);
                return false;
            }

            switch (header.DeviceType)
            {
                case DeviceType.Mouse:
                    break;

                case DeviceType.Keyboard:
                    var keyboard = Marshal.PtrToStructure<RAWKEYBOARD>(IntPtr.Add(dataBuffer, (int)rawHeaderSize));
                    rawInput = new RawKeyboardInput(header, keyboard);
                    break;

                case DeviceType.Hid:
                    break;
                default:
                    throw new NotImplementedException();
            }
            return true;
        }
        #endregion
    }
}
