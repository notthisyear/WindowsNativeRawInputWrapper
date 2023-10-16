using System;

namespace WindowsNativeRawInputWrapper.PlatformTypes
{
    internal class PlatformEnumerations
    {
        public enum RawInputDeviceType : uint
        {
            /// <summary>
            /// The device is a mouse.
            /// </summary>
            RIM_TYPEMOUSE = 0,

            /// <summary>
            /// The device is a keyboard.
            /// </summary>
            RIM_TYPEKEYBOARD = 1,

            /// <summary>
            /// The device is an HID that is not a keyboard and not a mouse.
            /// </summary>
            RIM_TYPEHID = 2
        }

        public enum GetDeviceInfoCommand : uint
        {
            /// <summary>
            /// pData is a PHIDP_PREPARSED_DATA pointer to a buffer for a <see href="https://learn.microsoft.com/en-us/windows-hardware/drivers/hid/top-level-collections">top-level collection's</see>
            /// <see href="https://learn.microsoft.com/en-us/windows-hardware/drivers/hid/preparsed-data">preparsed data</see>.
            /// </summary>
            RIDI_PREPARSEDDATA = 0x20000005,

            /// <summary>
            /// <para>pData points to a string that contains the <see href="https://learn.microsoft.com/en-us/windows-hardware/drivers/wdf/using-device-interfaces">device interface name.</see></para>
            /// <para>If this device is <see href="https://learn.microsoft.com/en-us/windows-hardware/drivers/hid/hid-architecture#hid-clients-supported-in-windows">opened with Shared Access Mode</see>
            /// then you can call <see href="https://learn.microsoft.com/en-us/windows/win32/api/fileapi/nf-fileapi-createfilew">CreateFile</see>
            /// with this name to open a HID collection and use returned handle for calling
            /// <see href="https://learn.microsoft.com/en-us/windows/win32/api/fileapi/nf-fileapi-readfile">ReadFile</see>
            /// to read input reports and <see href="https://learn.microsoft.com/en-us/windows/win32/api/fileapi/nf-fileapi-writefile">WriteFile</see> to send output reports.</para>
            /// <para>For more information, see
            /// <see href="https://learn.microsoft.com/en-us/windows-hardware/drivers/hid/opening-hid-collections">Opening HID Collections</see>
            /// and <see href="https://learn.microsoft.com/en-us/windows-hardware/drivers/hid/handling-hid-reports">Handling HID Reports.</see></para>
            /// <para>For this uiCommand only, the value in pcbSize is the character count (not the byte count).</para>
            /// </summary>
            RIDI_DEVICENAME = 0x20000007,

            /// <summary>
            /// pData points to an RID_DEVICE_INFO structure.
            /// </summary>
            RIDI_DEVICEINFO = 0x2000000b
        }

        public enum KeyboardType : uint
        {
            Enhanced101Or102KeyKeyboardOrCompatible = 0x4,
            JapaneseKeyboard = 0x7,
            KoreanKeyboard = 0x8,
            UnknownTypeOrHidKeyboard = 0x51
        }

        [Flags]
        public enum MouseDeviceIdentificationFlag : uint
        {
            /// <summary>
            /// <see href="https://learn.microsoft.com/en-us/windows-hardware/drivers/hid/keyboard-and-mouse-hid-client-drivers">HID mouse</see>.
            /// </summary>
            MOUSE_HID_HARDWARE = 0x0080,

            /// <summary>
            /// <see href="https://learn.microsoft.com/en-us/windows-hardware/drivers/hid/keyboard-and-mouse-hid-client-drivers">HID wheel mouse</see>.
            /// </summary>
            WHEELMOUSE_HID_HARDWARE = 0x0100,

            /// <summary>
            /// Mouse with horizontal wheel.
            /// </summary>
            HORIZONTAL_WHEEL_PRESENT = 0x8000,
        }

        [Flags]
        public enum DeviceRegistrationModeFlag : uint
        {
            /// <summary>
            /// If set, this removes the top level collection from the inclusion list. This tells the operating system to stop reading from a device which matches the top level collection.
            /// </summary>
            RIDEV_REMOVE = 0x00000001,

            /// <summary>
            /// If set, this specifies the top level collections to exclude when reading a complete usage page. This flag only affects a TLC whose usage page is already specified with RIDEV_PAGEONLY.
            /// </summary>
            RIDEV_EXCLUDE = 0x00000010,

            /// <summary>
            /// If set, this specifies all devices whose top level collection is from the specified usUsagePage. Note that usUsage must be zero. To exclude a particular top level collection, use RIDEV_EXCLUDE.
            /// </summary>
            RIDEV_PAGEONLY = 0x00000020,

            /// <summary>
            /// If set, this prevents any devices specified by usUsagePage or usUsage from generating legacy messages. This is only for the mouse and keyboard.
            /// </summary>
            RIDEV_NOLEGACY = 0x00000030,

            /// <summary>
            /// If set, this enables the caller to receive the input even when the caller is not in the foreground. Note that hwndTarget must be specified.
            /// </summary>
            RIDEV_INPUTSINK = 0x00000100,

            /// <summary>
            /// If set, the mouse button click does not activate the other window. RIDEV_CAPTUREMOUSE can be specified only if RIDEV_NOLEGACY is specified for a mouse device.
            /// </summary>
            RIDEV_CAPTUREMOUSE = 0x00000200,

            /// <summary>
            /// If set, the application-defined keyboard device hotkeys are not handled. However, the system hotkeys; for example, ALT+TAB and CTRL+ALT+DEL, are still handled. By default, all keyboard hotkeys are handled. RIDEV_NOHOTKEYS can be specified even if RIDEV_NOLEGACY is not specified and hwndTarget is NULL.
            /// </summary>
            RIDEV_NOHOTKEYS = RIDEV_CAPTUREMOUSE,

            /// <summary>
            /// If set, the application command keys are handled. RIDEV_APPKEYS can be specified only if RIDEV_NOLEGACY is specified for a keyboard device.
            /// </summary>
            RIDEV_APPKEYS = 0x00000400,

            /// <summary>
            /// <para>If set, this enables the caller to receive input in the background only if the foreground application does not process it. In other words, if the foreground application is not registered for raw input, then the background application that is registered will receive the input.</para>
            /// <para>Windows XP: This flag is not supported until Windows Vista</para>
            /// </summary>
            RIDEV_EXINPUTSINK = 0x00001000,

            /// <summary>
            /// <para>If set, this enables the caller to receive WM_INPUT_DEVICE_CHANGE notifications for device arrival and device removal.</para>
            /// <para>Windows XP: This flag is not supported until Windows Vista</para>
            /// </summary>
            RIDEV_DEVNOTIFY = 0x00002000
        }

        public enum RawInputReadCommand : uint
        {
            /// <summary>
            /// Get the header information from the RAWINPUT structure.
            /// </summary>
            RID_HEADER = 0x10000005,

            /// <summary>
            /// Get the raw data from the RAWINPUT structure.
            /// </summary>
            RID_INPUT = 0x10000003
        }

        [Flags]
        public enum MouseStateFlag : ushort
        {
            /// <summary>
            /// Mouse movement data is relative to the last mouse position.
            /// </summary>
            MOUSE_MOVE_RELATIVE = 0x00,

            /// <summary>
            /// Mouse movement data is based on absolute position.
            /// </summary>
            MOUSE_MOVE_ABSOLUTE = 0x01,

            /// <summary>
            /// Mouse coordinates are mapped to the virtual desktop(for a multiple monitor system).
            /// </summary>
            MOUSE_VIRTUAL_DESKTOP = 0x02,

            /// <summary>
            /// Mouse attributes changed; application needs to query the mouse attributes.
            /// </summary>
            MOUSE_ATTRIBUTES_CHANGED = 0x04,

            /// <summary>
            /// <para>This mouse movement event was not coalesced.Mouse movement events can be coalesced by default.</para>
            /// <para>Windows XP/2000: This value is not supported.</para>
            /// </summary>
            MOUSE_MOVE_NOCOALESCE = 0x08
        }

        [Flags]
        public enum MouseButtonFlag : ushort
        {
            /// <summary>
            /// Left button changed to down.
            /// </summary>
            RI_MOUSE_BUTTON_1_DOWN = 0x0001,
            RI_MOUSE_LEFT_BUTTON_DOWN = RI_MOUSE_BUTTON_1_DOWN,

            /// <summary>
            /// Left button changed to up.
            /// </summary>
            RI_MOUSE_BUTTON_1_UP = 0x0002,
            RI_MOUSE_LEFT_BUTTON_UP = RI_MOUSE_BUTTON_1_UP,

            /// <summary>
            /// Right button changed to down.
            /// </summary>
            RI_MOUSE_BUTTON_2_DOWN = 0x0004,
            RI_MOUSE_RIGHT_BUTTON_DOWN = RI_MOUSE_BUTTON_2_DOWN,

            /// <summary>
            /// Right button changed to up.
            /// </summary>
            RI_MOUSE_BUTTON_2_UP = 0x0008,
            RI_MOUSE_RIGHT_BUTTON_UP = RI_MOUSE_BUTTON_2_UP,

            /// <summary>
            /// Middle button changed to down.
            /// </summary>
            RI_MOUSE_BUTTON_3_DOWN = 0x0010,
            RI_MOUSE_MIDDLE_BUTTON_DOWN = RI_MOUSE_BUTTON_3_DOWN,

            /// <summary>
            /// Middle button changed to up.
            /// </summary>
            RI_MOUSE_BUTTON_3_UP = 0x0020,
            RI_MOUSE_MIDDLE_BUTTON_UP = RI_MOUSE_BUTTON_3_UP,

            /// <summary>
            /// XBUTTON1 changed to down.
            /// </summary>
            RI_MOUSE_BUTTON_4_DOWN = 0x0040,

            /// <summary>
            /// XBUTTON1 changed to up.
            /// </summary>
            RI_MOUSE_BUTTON_4_UP = 0x0080,

            /// <summary>
            /// XBUTTON2 changed to down.
            /// </summary>
            RI_MOUSE_BUTTON_5_DOWN = 0x0100,

            /// <summary>
            /// XBUTTON2 changed to up.
            /// </summary>
            RI_MOUSE_BUTTON_5_UP = 0x0200,

            /// <summary>
            /// <para>Raw input comes from a mouse wheel. The wheel delta is stored in ButtonData.</para>
            /// <para>A positive value indicates that the wheel was rotated forward, away from the user; a negative value indicates that the wheel was rotated backward, toward the user.</para>
            /// </summary>
            RI_MOUSE_WHEEL = 0x0400,

            /// <summary>
            /// <para>Raw input comes from a horizontal mouse wheel. The wheel delta is stored in ButtonData.</para>
            /// <para>A positive value indicates that the wheel was rotated to the right; a negative value indicates that the wheel was rotated to the left.</para>
            /// <para>Windows XP/2000: This value is not supported.</para>
            /// </summary>
            RI_MOUSE_HWHEEL = 0x0800
        }

        [Flags]
        public enum ScanCodeFlag : ushort
        {
            /// <summary>
            /// The key is down.
            /// </summary>
            RI_KEY_MAKE = 0x0000,

            /// <summary>
            /// The key is up.
            /// </summary>
            RI_KEY_BREAK = 0x0001,

            /// <summary>
            /// The scan code has the E0 prefix.
            /// </summary>
            RI_KEY_E0 = 0x0002,

            /// <summary>
            /// The scan code has the E1 prefix.
            /// </summary>
            RI_KEY_E1 = 0x0004

        }

        /// <summary>
        /// See <see href="https://www.usb.org/sites/default/files/documents/hut1_12v2.pdf"/> (page 14) for full list.
        /// </summary>
        public enum HidUsagePage : ushort
        {
            /// <summary>
            /// Generic desktop controls.
            /// </summary>
            GenericDesktopControls = 0x01
        }

        /// <summary>
        /// See <see href="https://www.usb.org/sites/default/files/documents/hut1_12v2.pdf"/> (page 26) for full list.
        /// </summary>
        public enum HidGenericDesktopControls : ushort
        {
            /// <summary>
            /// Pointer.
            /// </summary>
            HID_USAGE_GENERIC_POINTER = 0x01,

            /// <summary>
            /// Mouse.
            /// </summary>
            HID_USAGE_GENERIC_MOUSE = 0x02,

            /// <summary>
            /// Joystick.
            /// </summary>
            HID_USAGE_GENERIC_JOYSTICK = 0x04,

            /// <summary>
            /// Gamepad.
            /// </summary>
            HID_USAGE_GENERIC_GAMEPAD = 0x05,

            /// <summary>
            /// Keyboard.
            /// </summary>
            HID_USAGE_GENERIC_KEYBOARD = 0x06,

            /// <summary>
            /// Keypad.
            /// </summary>
            HID_USAGE_GENERIC_KEYPAD = 0x07,

            /// <summary>
            /// Multi-axis controller.
            /// </summary>
            HID_USAGE_GENERIC_MULTI_AXIS_CONTROLLER = 0x08
        }
    }
}
