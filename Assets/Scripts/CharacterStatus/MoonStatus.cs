using UnityEngine;
using System.Collections;

public class MoonStatus : CharacterStatus
{
    protected override void SetInitValue()
    {
        SetInitValue("Moon", 89, 83, 43, 38, 67, 1700);
    }

    protected override void SetGrowRate()
    {
        SetGrowRate(GrowRateType.Main, GrowRateType.Secondary, GrowRateType.Bad, GrowRateType.Bad, GrowRateType.Third);
    }
}
