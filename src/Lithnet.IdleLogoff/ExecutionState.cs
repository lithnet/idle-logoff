using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Lithnet.idlelogoff
{
    [Flags]
    public enum EXECUTION_STATE : ulong
    {
        NONE = 0,
        AWAYMODE_REQUIRED = 0x00000040,
        CONTINUOUS = 0x80000000,
        DISPLAY_REQUIRED = 0x00000002,
        SYSTEM_REQUIRED = 0x00000001,
    }
}