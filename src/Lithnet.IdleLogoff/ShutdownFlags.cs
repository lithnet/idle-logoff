namespace Lithnet.idlelogoff
{
    using System;

    [Flags]
    public enum ShutdownFlags : uint
    {
        Logoff = 0,
        Shutdown = 0x00000001,
        Reboot = 0x00000002,
        Force = 0x00000004,
        PowerOff = 0x00000008,
        ForceIfHung = 0x00000010
    }
}
