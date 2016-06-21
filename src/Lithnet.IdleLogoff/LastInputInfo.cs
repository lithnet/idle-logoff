namespace Lithnet.idlelogoff
{
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct LastInputInfo
    {
        public int cbSize;
        public int dwTime;
    }
}
