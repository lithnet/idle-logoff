using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Lithnet.idlelogoff
{
    [Flags]
    public enum ExecutionState : ulong
    {
        None = 0,
        AwaymodeRequired = 0x00000040,
        Continuous = 0x80000000,
        DisplayRequired = 0x00000002,
        SystemRequired = 0x00000001,
    }
}