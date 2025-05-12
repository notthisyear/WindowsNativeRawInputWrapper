using WindowsNativeRawInputWrapper.PlatformTypes;

namespace WindowsNativeRawInputWrapper
{
    internal abstract record UsagePageAndIdBase
    {
        public abstract PlatformEnumerations.HidUsagePage UsagePage { get; }

        public abstract ushort UsageId { get; }

        public static UsagePageAndIdBase GetGenericDesktopControlUsagePageAndFlag(PlatformEnumerations.HidGenericDesktopControls usageId)
            => new GenericDesktopControlUsagePageAndId(usageId);
    }

    internal record GenericDesktopControlUsagePageAndId : UsagePageAndIdBase
    {
        public override PlatformEnumerations.HidUsagePage UsagePage => PlatformEnumerations.HidUsagePage.GenericDesktopControls;

        public override ushort UsageId => (ushort)UsageIdType;

        public PlatformEnumerations.HidGenericDesktopControls UsageIdType { get; }

        public GenericDesktopControlUsagePageAndId(PlatformEnumerations.HidGenericDesktopControls usageIdType)
        {
            UsageIdType = usageIdType;
        }
    }
}
