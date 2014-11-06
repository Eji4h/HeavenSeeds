using UnityEngine;
using System.Collections;

public class YanaStatus : CharacterStatus
{
    protected override void SetInitValue()
    {
        SetInitValue("Yana", 42, 79, 94, 32, 64, 1650);
    }

    protected override void SetGrowRate()
    {
        SetGrowRate(GrowRateType.Bad, GrowRateType.Secondary, GrowRateType.Main, GrowRateType.Bad, GrowRateType.Third);
    }
}
