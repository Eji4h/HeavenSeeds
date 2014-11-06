using UnityEngine;
using System.Collections;

public class SenseiStatus : CharacterStatus
{
    protected override void SetInitValue()
    {
        SetInitValue("Sensei", 50, 52, 72, 77, 91, 1400);
    }

    protected override void SetGrowRate()
    {
        SetGrowRate(GrowRateType.Bad, GrowRateType.Bad, GrowRateType.Third, GrowRateType.Secondary, GrowRateType.Main);
    }
}
