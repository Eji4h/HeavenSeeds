using UnityEngine;
using System.Collections;

public class HansaStatus : CharacterStatus
{
    protected override void SetInitValue()
    {
        SetInitValue("Hansa", 84, 34, 61, 90, 50, 2250);
    }

    protected override void SetGrowRate()
    {
        SetGrowRate(GrowRateType.Secondary, GrowRateType.Bad, GrowRateType.Third, GrowRateType.Main, GrowRateType.Bad);
    }
}
