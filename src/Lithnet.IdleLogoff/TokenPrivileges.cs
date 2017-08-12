using System;
using System.Runtime.InteropServices;

namespace Lithnet.idlelogoff
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct TokenPrivileges
    {
        public int PrivilegeCount;
        public long Luid;
        public int Attributes;
    }
}
