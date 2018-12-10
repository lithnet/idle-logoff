using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lithnet.idlelogoff
{
    public enum PowerInformationLevel
    {
        AdministratorPowerPolicy = 9,
        LastSleepTime = 15,
        LastWakeTime = 14,
        ProcessorInformation = 11,
        ProcessorPowerPolicyAc = 18,
        ProcessorPowerPolicyCurrent = 22,
        ProcessorPowerPolicyDc = 19,
        SystemBatteryState = 5,
        SystemExecutionState = 16,
        SystemPowerCapabilities = 4,
        SystemPowerInformation = 12,
        SystemPowerPolicyAc = 0,
        SystemPowerPolicyCurrent = 8,
        SystemPowerPolicyDc = 1,
        SystemReserveHiberFile = 10,
        VerifyProcessorPowerPolicyAc = 20,
        VerifyProcessorPowerPolicyDc = 21,
        VerifySystemPolicyAc = 2,
        VerifySystemPolicyDc = 3
    }
}
