using UnityEngine;
using System.Collections;

public class ShunStatus : CharacterStatus
{
    protected override void SetInitValue()
    {
        SetInitValue("Shun", 92, 68, 45, 76, 37, 2000);
    }

    protected override void SetGrowRate()
    {
        SetGrowRate(GrowRateType.Main, GrowRateType.Third, GrowRateType.Bad, GrowRateType.Secondary, GrowRateType.Bad);
    }
}
