using System;
using WindowsNativeRawInputWrapper.PlatformTypes;

namespace WindowsNativeRawInputWrapper.Types
{
    public abstract record RawInputBase
    {
        public RawInputHeader Header { get; }

        protected RawInputBase(RawInputHeader header)
        {
            Header = header;
        }
    }

    public record RawKeyboardInput : RawInputBase
    {
        public enum KeyboardScanCode : ushort
        {
            UnknownScanCode = 0x0000,
            ErrorRollOver = 0x00FF,
            KeyboardA = 0x001E,
            KeyboardB = 0x0030,
            KeyboardC = 0x002E,
            KeyboardD = 0x0020,
            KeyboardE = 0x0012,
            KeyboardF = 0x0021,
            KeyboardG = 0x0022,
            KeyboardH = 0x0023,
            KeyboardI = 0x0017,
            KeyboardJ = 0x0024,
            KeyboardK = 0x0025,
            KeyboardL = 0x0026,
            KeyboardM = 0x0032,
            KeyboardN = 0x0031,
            KeyboardO = 0x0018,
            KeyboardP = 0x0019,
            KeyboardQ = 0x0010,
            KeyboardR = 0x0013,
            KeyboardS = 0x001F,
            KeyboardT = 0x0014,
            KeyboardU = 0x0016,
            KeyboardV = 0x002F,
            KeyboardW = 0x0011,
            KeyboardX = 0x002D,
            KeyboardY = 0x0015,
            KeyboardZ = 0x002C,
            Keyboard1AndBang = 0x0002,
            Keyboard2AndAt = 0x0003,
            Keyboard3AndHash = 0x0004,
            Keyboard4AndDollar = 0x0005,
            Keyboard5AndPercent = 0x0006,
            Keyboard6AndCaret = 0x0007,
            Keyboard7AndAmpersand = 0x0008,
            Keyboard8AndStar = 0x0009,
            Keyboard9AndLeftBracket = 0x000A,
            Keyboard0AndRightBracket = 0x000B,
            KeyboardReturnEnter = 0x001C,
            KeyboardEscape = 0x0001,
            KeyboardDelete = 0x000E,
            KeyboardTab = 0x000F,
            KeyboardSpacebar = 0x0039,
            KeyboardDashAndUnderscore = 0x000C,
            KeyboardEqualsAndPlus = 0x000D,
            KeyboardLeftBrace = 0x001A,
            KeyboardRightBrace = 0x001B,
            KeyboardPipeAndSlash = 0x002B,
            KeyboardSemiColonAndColon = 0x0027,
            KeyboardApostropheAndDoubleQuotationMark = 0x0028,
            KeyboardGraveAccentAndTilde = 0x0029,
            KeyboardComma = 0x0033,
            KeyboardPeriod = 0x0034,
            KeyboardQuestionMark = 0x0035,
            KeyboardCapsLock = 0x003A,
            KeyboardF1 = 0x003B,
            KeyboardF2 = 0x003C,
            KeyboardF3 = 0x003D,
            KeyboardF4 = 0x003E,
            KeyboardF5 = 0x003F,
            KeyboardF6 = 0x0040,
            KeyboardF7 = 0x0041,
            KeyboardF8 = 0x0042,
            KeyboardF9 = 0x0043,
            KeyboardF10 = 0x0044,
            KeyboardF11 = 0x0057,
            KeyboardF12 = 0x0058,
            KeyboardScrollLock = 0x0046,
            KeyboardInsert = 0xE052,
            KeyboardHome = 0xE047,
            KeyboardPageUp = 0xE049,
            KeyboardDeleteForward = 0xE053,
            KeyboardEnd = 0xE04F,
            KeyboardPageDown = 0xE051,
            KeyboardRightArrow = 0xE04D,
            KeyboardLeftArrow = 0xE04B,
            KeyboardDownArrow = 0xE050,
            KeyboardUpArrow = 0xE048,
            KeypadForwardSlash = 0xE035,
            KeypadStar = 0x0037,
            KeypadDash = 0x004A,
            KeypadPlus = 0x004E,
            KeypadENTER = 0xE01C,
            Keypad1AndEnd = 0x004F,
            Keypad2AndDownArrow = 0x0050,
            Keypad3AndPageDn = 0x0051,
            Keypad4AndLeftArrow = 0x004B,
            Keypad5 = 0x004C,
            Keypad6AndRightArrow = 0x004D,
            Keypad7AndHome = 0x0047,
            Keypad8AndUpArrow = 0x0048,
            Keypad9AndPageUp = 0x0049,
            Keypad0AndInsert = 0x0052,
            KeypadPeriod = 0x0053,
            KeyboardNonUSSlashBar = 0x0056,
            KeyboardApplication = 0xE05D,
            KeyboardPower = 0xE05E,
            KeypadEquals = 0x0059,
            KeyboardF13 = 0x0064,
            KeyboardF14 = 0x0065,
            KeyboardF15 = 0x0066,
            KeyboardF16 = 0x0067,
            KeyboardF17 = 0x0068,
            KeyboardF18 = 0x0069,
            KeyboardF19 = 0x006A,
            KeyboardF20 = 0x006B,
            KeyboardF21 = 0x006C,
            KeyboardF22 = 0x006D,
            KeyboardF23 = 0x006E,
            KeyboardF24 = 0x0076,
            KeyboardLeftControl = 0x001D,
            KeyboardLeftShift = 0x002A,
            KeyboardLeftAlt = 0x0038,
            KeyboardLeftGUI = 0xE05B,
            KeyboardRightControl = 0xE01D,
            KeyboardRightShift = 0x0036,
            KeyboardRightAlt = 0xE038,
            KeyboardRightGUI = 0xE05C
        }

        public KeyboardScanCode ScanCode { get; }

        public bool IsKeyDown { get; }

        public bool IsKeyUp { get; }

        internal RawKeyboardInput(RawInputHeader header, RAWKEYBOARD rawKeyboard) : base(header)
        {
            ScanCode = Enum.IsDefined(typeof(KeyboardScanCode), rawKeyboard.MakeCode)
                ? (KeyboardScanCode)rawKeyboard.MakeCode : KeyboardScanCode.UnknownScanCode;

            IsKeyDown = (rawKeyboard.Flags & (ushort)PlatformEnumerations.ScanCodeFlag.RI_KEY_MAKE) == (ushort)PlatformEnumerations.ScanCodeFlag.RI_KEY_MAKE;
            IsKeyUp = (rawKeyboard.Flags & (ushort)PlatformEnumerations.ScanCodeFlag.RI_KEY_BREAK) == (ushort)PlatformEnumerations.ScanCodeFlag.RI_KEY_BREAK;
        }
    }
}
