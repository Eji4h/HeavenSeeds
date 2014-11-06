using UnityEngine;
using System.Collections;

public class SansaStatus : CharacterStatus
{
    protected override void SetInitValue()
    {
        SetInitValue("Sansa", 58, 95, 39, 52, 73, 1800);
    }

    protected override void SetGrowRate()
    {
        SetGrowRate(GrowRateType.Third, GrowRateType.Main, GrowRateType.Bad, GrowRateType.Bad, GrowRateType.Secondary);
    }
}
